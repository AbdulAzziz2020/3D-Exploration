using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NPC;
using UnityEngine;

namespace Game
{
    public sealed class MemoryMatchCardPresenter : UIPresenter<MemoryMatchCardView>
    {
        private const int CardsPerPair = 2;

        [Header("Timing")]
        [SerializeField] private float m_checkDelay = 0.75f;
        [SerializeField] private float m_previewDelay = 1f;
        [SerializeField] private float m_hideDelay = 0.1f;

        private MemoryMatchCardComponent m_firstCard;
        private MemoryMatchCardComponent m_secondCard;

        private bool m_isChecking;
        private bool m_isPlaying;

        private int m_completedCount;
        private int m_totalCards;
        private int m_moves;

        private float m_elapsedTime;

        private CancellationTokenSource m_cts;
        private NpcMemoryMatchCard.CardData[] m_cards;
        private MemoryMatchModel m_memoryMatchModel;
        
        public event Action InteractFinish;

        private void Awake()
        {
            foreach (MemoryMatchCardComponent l_card in m_view.Cards)
            {
                l_card.Clicked += OnCardClicked;
            }

            m_view.Close += HandleClose;
            m_view.Restart += HandleRestart;
        }

        private void OnDestroy()
        {
            m_cts?.Cancel();
            m_cts?.Dispose();

            foreach (MemoryMatchCardComponent l_card in m_view.Cards)
            {
                l_card.Clicked -= OnCardClicked;
            }

            m_view.Close -= HandleClose;
            m_view.Restart -= HandleRestart;
        }

        private void HandleClose()
        {
            EndGame();
            InteractFinish?.Invoke();
            Hide();
        }

        private void HandleRestart()
        {
            Setup(m_cards, m_memoryMatchModel);
        }

        public void Setup(NpcMemoryMatchCard.CardData[] p_cards, MemoryMatchModel p_memoryMatchModel)
        {
            m_cards = p_cards;
            m_memoryMatchModel = p_memoryMatchModel;
            
            m_cts?.Cancel();
            m_cts?.Dispose();
            m_cts = new CancellationTokenSource();

            m_firstCard = null;
            m_secondCard = null;

            m_isChecking = true;
            m_isPlaying = false;

            m_completedCount = 0;
            m_moves = 0;
            m_elapsedTime = 0f;

            m_totalCards = p_cards.Length * CardsPerPair;

            m_view.CompletePanel(p_isShow: false);
            m_view.BoardPanel(p_isShow: true);
            
            m_view.RenderBestScore(p_memoryMatchModel);
            m_view.SetMoves(m_moves);
            m_view.SetTimer(m_elapsedTime);

            SetupCards(p_cards);

            Show();

            PreviewCardsAsync(m_cts.Token).Forget();
        }

        private void SetupCards(NpcMemoryMatchCard.CardData[] p_cards)
        {
            RuntimeCard[] l_cards = CreateRuntimeCards(p_cards);

            Shuffle(l_cards);

            for (int i = 0; i < l_cards.Length; i++)
            {
                RuntimeCard l_data = l_cards[i];

                m_view.Cards[i].Setup(
                    i,
                    l_data.PairId,
                    l_data.Sprite);

                m_view.Cards[i].SetInteractable(false);
            }
        }

        private RuntimeCard[] CreateRuntimeCards(NpcMemoryMatchCard.CardData[] p_cards)
        {
            RuntimeCard[] l_cards = new RuntimeCard[p_cards.Length * CardsPerPair];

            int l_index = 0;

            for (int i = 0; i < p_cards.Length; i++)
            {
                NpcMemoryMatchCard.CardData l_data = p_cards[i];

                for (int j = 0; j < CardsPerPair; j++)
                {
                    l_cards[l_index] = new RuntimeCard
                    {
                        PairId = l_data.pairId,
                        Sprite = l_data.sprite
                    };

                    l_index++;
                }
            }

            return l_cards;
        }

        private void Shuffle(RuntimeCard[] p_cards)
        {
            for (int i = p_cards.Length - 1; i > 0; i--)
            {
                int l_randomIndex = UnityEngine.Random.Range(0, i + 1);

                RuntimeCard l_temp = p_cards[i];

                p_cards[i] = p_cards[l_randomIndex];
                p_cards[l_randomIndex] = l_temp;
            }
        }

        private async UniTaskVoid PreviewCardsAsync(CancellationToken p_token)
        {
            for (int i = 0; i < m_totalCards; i++)
            {
                m_view.Cards[i].Reveal();
            }

            await UniTask.Delay(Mathf.RoundToInt(m_previewDelay * 1000f), cancellationToken: p_token);

            for (int i = 0; i < m_totalCards; i++)
            {
                m_view.Cards[i].Hidden();

                await UniTask.Delay(Mathf.RoundToInt(m_hideDelay * 1000f), cancellationToken: p_token);
            }

            StartGame();
        }

        private void StartGame()
        {
            if (m_cts == null)
                return;

            m_isChecking = false;
            m_isPlaying = true;

            for (int i = 0; i < m_totalCards; i++)
            {
                m_view.Cards[i].SetInteractable(true);
            }

            UpdateTimerAsync(m_cts.Token).Forget();
        }

        private void OnCardClicked(MemoryMatchCardComponent p_card)
        {
            if (!m_isPlaying || m_isChecking)
                return;

            p_card.Reveal();

            if (m_firstCard == null)
            {
                m_firstCard = p_card;
                return;
            }

            m_secondCard = p_card;

            m_moves++;

            m_view.SetMoves(m_moves);

            CheckPairAsync(m_cts.Token).Forget();
        }

        private async UniTaskVoid CheckPairAsync(CancellationToken p_token)
        {
            m_isChecking = true;

            await UniTask.Delay(Mathf.RoundToInt(m_checkDelay * 1000f), cancellationToken: p_token);

            if (m_firstCard == null ||
                m_secondCard == null)
            {
                return;
            }

            bool l_isMatch =
                m_firstCard.PairId == m_secondCard.PairId;

            if (l_isMatch)
            {
                m_firstCard.Complete();
                m_secondCard.Complete();

                m_completedCount += CardsPerPair;

                ClearSelection();

                if (m_completedCount >= m_totalCards)
                {
                    Complete();
                }
            }
            else
            {
                m_firstCard.Hidden();
                m_secondCard.Hidden();

                ClearSelection();
            }
        }

        private async UniTaskVoid UpdateTimerAsync(CancellationToken p_token)
        {
            while (m_isPlaying)
            {
                await UniTask.Yield(
                    PlayerLoopTiming.Update,
                    p_token);

                m_elapsedTime += Time.deltaTime;

                m_view.SetTimer(m_elapsedTime);
            }
        }

        private void ClearSelection()
        {
            m_firstCard = null;
            m_secondCard = null;
            m_isChecking = false;
        }

        private void Complete()
        {
            EndGame();

            Debug.Log($"Memory Match Complete - Moves: {m_moves}, Time: {m_elapsedTime:0.00}s");

            SaveBestScore().Forget();
        }

        private async UniTask SaveBestScore()
        {
            UserModel l_model = await PlayerSave.Singleton.GetAsync();
            bool l_isBetter = l_model.MemoryMatch.IsBetterThan(m_moves, m_elapsedTime);

            if (l_isBetter)
            {
                l_model.MemoryMatch.SetScore(m_moves, m_elapsedTime, DateTime.UtcNow.ToString("O"));
                await PlayerSave.Singleton.SaveAsync();
                
                m_view.RenderBestScore(l_model.MemoryMatch);
            }
        }

        private void EndGame()
        {
            if (!m_isPlaying && !m_isChecking)
                return;

            m_isPlaying = false;
            m_isChecking = false;
            
            m_view.CompletePanel(p_isShow: true);
            m_view.BoardPanel(p_isShow: false);

            SetInteractable(false);
        }

        private void SetInteractable(bool p_value)
        {
            foreach (MemoryMatchCardComponent l_card in m_view.Cards)
            {
                l_card.SetInteractable(p_value);
            }
        }

        private struct RuntimeCard
        {
            public int PairId;
            public Sprite Sprite;
        }
    }
}