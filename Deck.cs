using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CardCeption
{
    public class Deck
    {
        private static Card[] arr;
        private static Random rg = new Random();
        private static int count = 0;

        public Deck()
        {
            arr = new Card[0];
        }
        public Card[] Arr { get => arr; set => arr = value; }
        public int Count { get => count; set => count = value; }

        public void AddDeck(List<Card> cards)
        {
            for(int i = 0; i < arr.Length; i += 2)
            {
                arr[i] = new Card(cards[i / 2].Id, cards[i / 2].Texture, 9, 15);
                arr[i + 1] = new Card(cards[i / 2].Id, cards[i / 2].Texture, 9, 15);
                count++;
            }

            ShuffleDeck();
        }

        public void ShuffleDeck()
        {
            int n = arr.Length;

            for(int i = 0; i < arr.Length; i++)
            {
                Card temp = arr[i];
                int rand = rg.Next(n--);
                arr[i] = arr[rand];
                arr[rand] = temp;
            }
        }

        public static List<Card> Shuffle(List<Card> cards)
        {
            int n = cards.Count;

            for(int i = 0; i < cards.Count; i++)
            {
                Card temp = cards[i];
                int rand = rg.Next(n--);

                cards[i] = cards[rand];
                cards[rand] = temp;
            }

            return cards;
        }

        public static void ClearDeck()
        {
            Array.Clear(arr);
            count = 0;
        }

        public void SetDeck(int amount)
        {
            ClearDeck();
            arr = new Card[amount];
        }
    }
}
