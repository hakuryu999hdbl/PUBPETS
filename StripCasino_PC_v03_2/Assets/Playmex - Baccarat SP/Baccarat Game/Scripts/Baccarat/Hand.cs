using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
namespace Baccarat_Game
{
    [System.Serializable]
    public class Hand : MonoBehaviour
    {

        protected Transform handPosition;
        protected Transform drawPosition;

        public GameObject scoreOutput;
        public GameObject scorePanel;

        [Range(0.5f, 0.6f)]
        public float cardXShift = 0.52f;

        public int currentScore = 0;

        protected List<GameObject> tableCards;

        protected Vector3 nextTransformPosition;

        private void Awake()
        {
            tableCards = new List<GameObject>();
            handPosition = transform.GetChild(0);
        }

        void Start()
        {
            ResetHand();
            drawPosition = GameObject.Find("/DrawPosition").transform;
            if (scoreOutput == null || scorePanel == null)
                return;
            scoreOutput.SetActive(false);
            scorePanel.SetActive(false);
        }

        public GameObject DealCard(CardData card)
        {
            return InitialiseInShoe(card);
        }

        public void ResetScore()
        {
            currentScore = 0;

            UpdateScoreView();

            if (scoreOutput == null || scorePanel == null)
                return;
            scoreOutput.SetActive(false);
            scorePanel.SetActive(false);
        }

        public virtual void ResetHand()
        {
            nextTransformPosition = handPosition.transform.position;
            tableCards.Clear();
            ResetScore();
        }

        public bool IsBust()
        {
            return currentScore > 21;
        }

        //手动修改
        public GameObject Card;
        protected GameObject InitialiseInShoe(CardData card)
        {
            //GameObject cardObj = Resources.Load<GameObject>("Card");


            if (Card == null)
            {
                Debug.LogError("Card prefab is not assigned on " + gameObject.name);
                return null;
            }

            //GameObject cardObj = Card;

            GameObject newCardGameObject = Instantiate(Card, drawPosition.position, drawPosition.rotation) as GameObject;

            TableResetManager.AddCard(newCardGameObject);

            newCardGameObject.GetComponentInChildren<MeshFilter>().mesh = card.GetMesh();

            LinkCardWithData(newCardGameObject, card);

            AddCardToHand(newCardGameObject);

            return newCardGameObject;
        }

        protected void AddCardToHand(GameObject card)
        {
            card.transform.SetParent(handPosition.parent);

            SetupPosition(card);

            tableCards.Add(card);
            CalculateScore();
        }

        public bool HasPerfectPair()
        {
            return tableCards[0].GetComponentInChildren<Card>().cardData.suit.Equals(tableCards[1].GetComponentInChildren<Card>().cardData.suit) &&
                tableCards[0].GetComponentInChildren<Card>().cardData.rank.Equals(tableCards[1].GetComponentInChildren<Card>().cardData.rank);
        }

        public bool HasPair()
        {
            return tableCards[0].GetComponentInChildren<Card>().cardData.rank.Equals(tableCards[1].GetComponentInChildren<Card>().cardData.rank);
        }

        public void UpdateScoreView()
        {
            if (scoreOutput == null || scorePanel == null)
                return;

            if (currentScore < 0)
            {
                scoreOutput.SetActive(false);
                scorePanel.SetActive(false);
                return;
            }

            scoreOutput.SetActive(true);
            scorePanel.SetActive(true);

            scoreOutput.GetComponent<TMP_Text>().text = currentScore.ToString();
        }

        protected void LinkCardWithData(GameObject cardGameObject, CardData cardData)
        {
            cardGameObject.GetComponentInChildren<Card>().cardData = cardData;
        }

        protected virtual void SetupPosition(GameObject cardGameObject)
        {
            //cardGameObject.GetComponent<Card>().endPosition = nextTransformPosition;
            Flip flipType = GetFlipType(tableCards.Count > 1);
            Vector3 flipPosition = Vector3.zero;
            if (flipType == Flip.Horizontal_FlipUp)
                flipPosition = new Vector3(0, 90, 0);

            Sequence sq = DOTween.Sequence();
            sq
                .Append(cardGameObject.transform.DOMove(drawPosition.GetChild(0).position, .4f).SetEase(Ease.OutSine))
                .Join(cardGameObject.transform.DORotate(drawPosition.GetChild(0).rotation.eulerAngles, .2f).SetEase(Ease.InSine))
                .Append(cardGameObject.transform.DOJump(nextTransformPosition, .15f, 1, .6f).SetEase(Ease.OutSine))
                .Join(cardGameObject.transform.DORotate(flipPosition, .2f).SetEase(Ease.OutSine))
                .OnComplete(() =>
                {
                    DealQueue.ProcessCard();
                    UpdateScoreView();
                });

            UpdateNextPosition();
        }

        protected virtual void UpdateNextPosition()
        {
            float offset = (tableCards.Count == 1) ? 0.10f : 0;
            nextTransformPosition = nextTransformPosition - new Vector3(-cardXShift - offset, -0.0001f, offset);
        }

        public void CalculateScore()
        {
            currentScore = 0;
            Card card;

            foreach (GameObject cardGO in tableCards)
            {
                card = cardGO.GetComponentInChildren<Card>();
                currentScore += card.cardData.GetValue();
            }
            currentScore %= 10;
        }

        public int GetCurrentScore()
        {
            return currentScore %= 10;
        }

        protected virtual Flip GetFlipType(bool isThirdCard)
        {
            if (isThirdCard)
                return Flip.Horizontal_FlipUp;

            return Flip.FlipUp;
        }

        public int GetNumberOfCards()
        {
            return tableCards.Count;
        }

    }

}