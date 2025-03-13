using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
namespace Blackjack_Game
{
    public enum Outcome { Win, NoWin, Push, Bust, Blackjack }

    [System.Serializable]
    public class Hand : MonoBehaviour
    {

        public GameObject scorePanel;
        [HideInInspector]
        public TMP_Text scoreOutput;

        public Vector3 cardShift = new Vector3(0.15f, 0.001f, 0.05f);

        public int currentScore = 0;
        private int diminishedScore = 0;

        protected List<Card> _Cards;
        protected Vector3 nextTransformPosition;
        protected Vector3 originalPosition;

        [Space]
        [Header("Outcome")]
        public GameObject outcomePanel;
        public TMP_Text outcomeText;

        private void Awake()
        {
            _Cards = new List<Card>();
        }

        void Start()
        {
            scoreOutput = scorePanel.GetComponentInChildren<TMP_Text>();
            outcomeText = outcomePanel.GetComponentInChildren<TMP_Text>();
            outcomePanel.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            originalPosition = transform.position;
            ResetHand();
        }

        public GameObject DealCard(CardData card, FlipType flip)
        {

            GameObject newCardGameObject = InstanciateCard(card, flip);
            AddCardToHand(newCardGameObject);
            SetupPosition(newCardGameObject, flip);

            return newCardGameObject;
        }

        public void ResetScore()
        {
            currentScore = 0;

            UpdateScoreView();

            if (scoreOutput == null || scorePanel == null)
                return;

            scorePanel.SetActive(false);
        }

        public virtual void ResetHand()
        {
            outcomePanel.SetActive(false);
            nextTransformPosition = transform.position;
            _Cards.Clear();
            ResetScore();
        }

        public bool IsBust()
        {
            bool busted = currentScore > 21;

            if (busted)
                ShowOutCome(Outcome.Bust);

            return busted;
        }

        public bool IsPush(int otherScore)
        {
            bool push = currentScore == otherScore;

            if (push)
                ShowOutCome(Outcome.Push);

            return push;
        }

        public bool IsPush(int otherScore, bool otherHasBlackjack)
        {
            if (otherHasBlackjack)
            {
                return Check_Blackjack();
            }

            bool push = (currentScore == otherScore);

            if (push)
                ShowOutCome(Outcome.Push);

            return push;
        }

        public bool Has_Blackjack()
        {
            bool blackjack = (_Cards.Count == 2 && currentScore == 21);

            if (blackjack)
                ShowOutCome(Outcome.Blackjack);

            return blackjack;
        }


        public bool Check_Blackjack()
        {
            return (_Cards.Count == 2 && currentScore == 21);
        }

        public void ShowOutCome(Outcome outcome)
        {
            outcomePanel.SetActive(true);
            outcomePanel.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            switch (outcome)
            {
                case Outcome.Win:
                    outcomeText.text = "Win";
                    outcomeText.color = Color.yellow;
                    break;
                case Outcome.NoWin:
                    outcomeText.text = "No Win";
                    outcomeText.color = Color.white;
                    break;
                case Outcome.Push:
                    outcomeText.text = "Push";
                    outcomeText.color = new Color(.2f, .8f, 1f);
                    break;
                case Outcome.Bust:
                    outcomeText.text = "Bust";
                    outcomeText.color = Color.red;
                    break;
                case Outcome.Blackjack:
                    outcomeText.text = "Blackjack";
                    outcomeText.color = Color.yellow;
                    break;
                default:
                    outcomePanel.SetActive(false);
                    break;
            }
        }

        protected void AddCardToHand(GameObject card)
        {
            card.transform.SetParent(transform);

        }

        public bool HasSplitPair()
        {
            if (_Cards.Count != 2)
                return false;

            return _Cards[0].cardData.rank.Equals(_Cards[1].cardData.rank);
        }

        public bool HasSplitAcesPair()
        {
            if (_Cards.Count != 2)
                return false;

            return _Cards[0].cardData.rank == _Cards[1].cardData.rank && _Cards[0].cardData.rank == CardData.Rank.Ace;
        }

        private GameObject InstanciateCard(CardData card, FlipType flip)
        {
            GameObject cardObj = Resources.Load<GameObject>("Card");
            Transform drawPoint = TableResetManager.GetDrawPoint();

            GameObject newCardGameObject = Instantiate(cardObj, drawPoint.position, drawPoint.rotation) as GameObject;
            newCardGameObject.transform.SetParent(transform);

            newCardGameObject.GetComponentInChildren<MeshFilter>().mesh = card.GetMesh();

            Card tmp_card = newCardGameObject.GetComponent<Card>();
            tmp_card.cardData = card;
            tmp_card.side = flip;
            _Cards.Add(tmp_card);
            TableResetManager.AddCard(newCardGameObject);

            return newCardGameObject;
        }

        protected virtual void SetupPosition(GameObject cardGameObject, FlipType flip)
        {
            Vector3 side = flip == FlipType.FlipUp ? new Vector3(0, 180, 0) : new Vector3(0, 180, 180);

            Vector3 pos = cardShift * (_Cards.Count - 1) + transform.position;
            Transform drawPoint = TableResetManager.GetDrawPoint();

            Sequence sq = DOTween.Sequence();
            sq
                .Append(cardGameObject.transform.DOMove(drawPoint.GetChild(0).position, .4f).SetEase(Ease.OutSine))
                .Join(cardGameObject.transform.DORotate(drawPoint.GetChild(0).rotation.eulerAngles, .2f).SetEase(Ease.InSine))
                .Append(cardGameObject.transform.DOJump(pos, .03f, 1, .6f).SetEase(Ease.OutSine))
                .Join(cardGameObject.transform.DOLocalRotate(side, .3f).SetDelay(.1f).SetEase(Ease.OutSine))
                .OnComplete(() =>
                {
                    UpdateScoreView();
                    DealQueue.ProcessCard();
                });


            UpdateNextPosition();
        }

        protected virtual void AnimateToPosition(GameObject cardGameObject, FlipType flip)
        {
            Vector3 side = flip == FlipType.FlipUp ? new Vector3(0, 180, 0) : new Vector3(0, 180, 180);
            Sequence sq = DOTween.Sequence();
            sq
                .Append(cardGameObject.transform.DOLocalRotate(side, .3f).SetEase(Ease.OutSine))
                .Join(cardGameObject.transform.DOLocalMove(Vector3.zero, .3f).SetEase(Ease.OutSine))
                .OnComplete(() =>
                {
                    UpdateScoreView();
                    DealQueue.ProcessCard();
                });

            UpdateNextPosition();
        }

        protected virtual void UpdateNextPosition()
        {
            nextTransformPosition = cardShift * (_Cards.Count - 1) + transform.position;
        }

        public void CalculateScore()
        {
            currentScore = 0;
            int aceCount = 0;

            foreach (Card card in _Cards)
            {
                if (card.side == FlipType.FlipUp)
                {
                    if (card.cardData.rank != CardData.Rank.Ace)
                    {
                        currentScore += card.cardData.GetValue();
                    }
                    else
                    {
                        aceCount++;
                    }
                }
            }

            if (aceCount > 0)
                diminishedScore = currentScore + aceCount;
            else
                diminishedScore = 0;

            for (int i = 0; i < aceCount; i++)
            {
                if (currentScore + 11 < 22)
                {
                    currentScore += 11;
                }
                else
                {
                    currentScore++;
                }
            }
        }

        public bool MightHave21()
        {
            return (_Cards[0].cardData.rank == CardData.Rank.Ace);
        }

        public void UpdateScoreView()
        {
            CalculateScore();
            if (currentScore <= 0)
            {
                scorePanel.SetActive(false);
                return;
            }
            if (currentScore < 21)
                scoreOutput.color = Color.white;
            else
                scoreOutput.color = currentScore == 21 ? Color.yellow : Color.red;

            scorePanel.SetActive(true);

            if (Check_Blackjack())
            {
                scoreOutput.text = "BJ";
                scoreOutput.fontSize = .55f;
                return;
            }

            if (diminishedScore > 0 && currentScore != 21 && currentScore != diminishedScore)
            {
                scoreOutput.text = diminishedScore.ToString("0") + "/" + currentScore.ToString("0");
                scoreOutput.fontSize = .35f;
            }
            else
            {
                scoreOutput.text = currentScore.ToString("0");
                scoreOutput.fontSize = .55f;
            }
        }

        public int NumberOfCards()
        {
            return _Cards.Count;
        }

        public Card GetSecondCard()
        {
            return _Cards[1];
        }

        void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, new Vector3(.12f, .005f, .15f));
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + cardShift, new Vector3(.12f, .005f, .15f));
        }
    }
}