using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public sealed class SlidingPuzzlePresenter : UIPresenter<SlidingPuzzleView>
    {
        private const int BoardSize = 3;
        private const int TileCount = BoardSize * BoardSize;
        private const int EmptyTileId = 0;

        private readonly int[] m_board = new int[TileCount];

        private int m_emptyIndex;
        private int m_moves;
        private float m_elapsedTime;

        private bool m_isPlaying;

        public event Action InteractFinish;

        private CancellationTokenSource m_cts;
        private SlidingPuzzleModel m_slidingPuzzleModel;
        private int m_shuffleMoves;

        private void Awake()
        {
            if (m_view.Tiles.Length != TileCount)
            {
                Debug.LogError(
                    $"Sliding Puzzle requires exactly {TileCount} tiles.",
                    this);

                return;
            }

            foreach (SlidingPuzzleComponent l_tile in m_view.Tiles)
            {
                l_tile.Clicked += OnTileClicked;
            }

            m_view.Close += HandleClose;
            m_view.Restart += HandleRestart;
        }

        private void OnDestroy()
        {
            m_cts?.Cancel();
            m_cts?.Dispose();

            foreach (SlidingPuzzleComponent l_tile in m_view.Tiles)
            {
                l_tile.Clicked -= OnTileClicked;
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
            Setup(m_shuffleMoves, m_slidingPuzzleModel);
        }

        public void Setup(int p_shuffleMoves, SlidingPuzzleModel p_slidingPuzzleModel)
        {
            m_shuffleMoves = p_shuffleMoves;
            m_slidingPuzzleModel = p_slidingPuzzleModel;
            
            m_cts?.Cancel();
            m_cts?.Dispose();
            m_cts = new CancellationTokenSource();

            m_moves = 0;
            m_elapsedTime = 0f;
            m_emptyIndex = TileCount - 1;
            m_isPlaying = false;

            CreateSolvedBoard();
            Shuffle(p_shuffleMoves);

            RefreshView();

            m_view.CompletePanel(p_isShow: false);
            m_view.BoardPanel(p_isShow: true);
            
            m_view.RenderBestScore(p_slidingPuzzleModel);
            m_view.SetMoves(m_moves);
            m_view.SetTimer(m_elapsedTime);

            SetInteractable(false);

            Show();

            m_isPlaying = true;
            SetInteractable(true);

            UpdateTimerAsync(m_cts.Token).Forget();
        }

        private void CreateSolvedBoard()
        {
            for (int i = 0; i < TileCount - 1; i++)
            {
                m_board[i] = i + 1;
            }

            m_board[TileCount - 1] = EmptyTileId;
            m_emptyIndex = TileCount - 1;
        }

        private void Shuffle(int p_moves)
        {
            if (p_moves <= 0)
                return;

            int l_previousEmptyIndex = -1;

            for (int i = 0; i < p_moves; i++)
            {
                int l_randomIndex;

                do
                {
                    l_randomIndex =
                        GetRandomNeighborIndex(m_emptyIndex);
                }
                while (l_randomIndex == l_previousEmptyIndex);

                Swap(
                    l_randomIndex,
                    m_emptyIndex);

                l_previousEmptyIndex = m_emptyIndex;
                m_emptyIndex = l_randomIndex;
            }

            if (IsSolved())
            {
                int l_randomIndex =
                    GetRandomNeighborIndex(m_emptyIndex);

                Swap(
                    l_randomIndex,
                    m_emptyIndex);

                m_emptyIndex = l_randomIndex;
            }
        }

        private int GetRandomNeighborIndex(int p_index)
        {
            int l_row = p_index / BoardSize;
            int l_column = p_index % BoardSize;

            int l_count = 0;

            if (l_row > 0)
                l_count++;

            if (l_row < BoardSize - 1)
                l_count++;

            if (l_column > 0)
                l_count++;

            if (l_column < BoardSize - 1)
                l_count++;

            int l_random = Random.Range(0, l_count);

            if (l_row > 0)
            {
                if (l_random == 0)
                    return p_index - BoardSize;

                l_random--;
            }

            if (l_row < BoardSize - 1)
            {
                if (l_random == 0)
                    return p_index + BoardSize;

                l_random--;
            }

            if (l_column > 0)
            {
                if (l_random == 0)
                    return p_index - 1;

                l_random--;
            }

            return p_index + 1;
        }

        private void OnTileClicked(SlidingPuzzleComponent p_tile)
        {
            if (!m_isPlaying)
                return;

            int l_tileIndex = p_tile.Index;

            if (!IsAdjacent(
                    l_tileIndex,
                    m_emptyIndex))
            {
                return;
            }

            MoveTile(l_tileIndex);

            m_moves++;

            m_view.SetMoves(m_moves);

            RefreshView();

            if (IsSolved())
            {
                Complete();
            }
        }

        private void MoveTile(int p_tileIndex)
        {
            Swap(
                p_tileIndex,
                m_emptyIndex);

            m_emptyIndex = p_tileIndex;
        }

        private bool IsAdjacent(
            int p_firstIndex,
            int p_secondIndex)
        {
            int l_firstRow =
                p_firstIndex / BoardSize;

            int l_firstColumn =
                p_firstIndex % BoardSize;

            int l_secondRow =
                p_secondIndex / BoardSize;

            int l_secondColumn =
                p_secondIndex % BoardSize;

            int l_rowDistance =
                Mathf.Abs(
                    l_firstRow - l_secondRow);

            int l_columnDistance =
                Mathf.Abs(
                    l_firstColumn - l_secondColumn);

            return l_rowDistance + l_columnDistance == 1;
        }

        private void Swap(
            int p_firstIndex,
            int p_secondIndex)
        {
            int l_temp =
                m_board[p_firstIndex];

            m_board[p_firstIndex] =
                m_board[p_secondIndex];

            m_board[p_secondIndex] =
                l_temp;
        }

        private void RefreshView()
        {
            for (int i = 0; i < TileCount; i++)
            {
                m_view.Tiles[i].Setup(
                    i,
                    m_board[i]);
            }
        }

        private bool IsSolved()
        {
            for (int i = 0; i < TileCount - 1; i++)
            {
                if (m_board[i] != i + 1)
                    return false;
            }

            return m_board[TileCount - 1] == EmptyTileId;
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

        private void Complete()
        {
            EndGame();

            Debug.Log($"Sliding Puzzle Complete - Moves: {m_moves}, Time: {m_elapsedTime:0.00}s");

            SaveBestScore().Forget();
        }

        private async UniTask SaveBestScore()
        {
            UserModel l_model = await PlayerSave.Singleton.GetAsync();
            bool l_isBetter = l_model.SlidingPuzzle.IsBetterThan(m_moves, m_elapsedTime);

            if (l_isBetter)
            {
                l_model.SlidingPuzzle.SetScore(m_moves, m_elapsedTime, DateTime.UtcNow.ToString("O"));
                await PlayerSave.Singleton.SaveAsync();
                
                m_view.RenderBestScore(l_model.SlidingPuzzle);
            }
        }

        private void EndGame()
        {
            if (!m_isPlaying)
                return;

            m_isPlaying = false;
            
            m_view.CompletePanel(p_isShow: true);
            m_view.BoardPanel(p_isShow: false);

            SetInteractable(false);
        }

        private void SetInteractable(bool p_value)
        {
            foreach (SlidingPuzzleComponent l_tile in m_view.Tiles)
            {
                l_tile.SetInteractable(p_value);
            }
        }
    }
}