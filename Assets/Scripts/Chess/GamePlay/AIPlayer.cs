namespace Chess
{
    using System;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Collections;
    using UnityEngine;

    public class AIPlayer : Player
    {

        const int bookMoveDelayMillis = 250;

        Search search;
        AISettings settings;
        bool moveFound;
        Move move;
        Board board;
        CancellationTokenSource cancelSearchTimer;

        Book book;

        public AIPlayer(Board board, AISettings settings)
        {
            this.settings = settings;
            this.board = board;
            search = new Search(board, settings);
            search.onSearchComplete += OnSearchComplete;
            search.searchDiagnostics = new Search.SearchDiagnostics();
            book = BookCreator.LoadBookFromFile(settings.book);
        }

        // Update running on Unity main thread. This is used to return the chosen move so as
        // not to end up on a different thread and unable to interface with Unity stuff.
        public override void Update()
        {
            if (moveFound)
            {
                moveFound = false;
                DoMove(move);
            }

            settings.diagnostics = search.searchDiagnostics;

        }

        public override void FinishThisTurn()
        {

            search.searchDiagnostics.isBook = false;
            moveFound = false;

            Move bookMove = Move.InvalidMove;

            if (settings.useBook && board.plyCount <= settings.maxBookPly)
            {
                if (book.HasPosition(board.ZobristKey))
                {
                    //bookMove = book.GetRandomBookMoveWeighted(currentBoard.ZobristKey);
                    bookMove = book.GetRandomBookMove(board.ZobristKey);
                }
            }

            if (bookMove.IsInvalid)
            {
                    StartSearch();
            }
            else
            {

                search.searchDiagnostics.isBook = true;
               
                settings.diagnostics = search.searchDiagnostics;
                Task.Delay(bookMoveDelayMillis).ContinueWith((t) => PlayBookMove(bookMove));

            }
        }

        void StartSearch()
        {
            search.StartSearch();
            moveFound = true;
        }

        

      

        void PlayBookMove(Move bookMove)
        {
            this.move = bookMove;
            moveFound = true;
        }

        void OnSearchComplete(Move move)
        {
            // Cancel search timer in case search finished before timer ran out (can happen when a mate is found)
            cancelSearchTimer?.Cancel();
            moveFound = true;
            this.move = move;
        }

    }
}