using General.State;
using UnityEngine;

namespace Data.Items
{
    public abstract class Item : HighPriorityState
    {
        public int MaxCapacity;

        public string Description;

        public Sprite normalSprite;

        public Sprite highlightedSprite;

        public abstract Item UseItem(Transform objectThatUsesItem);
    }
}
