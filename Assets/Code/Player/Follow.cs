using Map;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

namespace Player
{
    public class Follow : MonoBehaviour
    {

        [Serializable]
        private class FollowItem
        {

            public Transform item;

            public LinkedList<Vector2> RecordPoints;

            public float Distance
            {

                get
                {

                    float Result = 0;

                    LinkedListNode<Vector2> P = RecordPoints.First;

                    while (P.Next != null)
                    {

                        Result += Vector2.Distance(P.Value, P.Next.Value);

                        P = P.Next;

                    }

                    return Result;

                }

            }

            public float Speed;

            public FollowItem(Transform newItem)
            {

                item = newItem;

                RecordPoints = new LinkedList<Vector2>();

                RecordPoints.AddLast(item.position);

            }

            public void TryRecordPoint(float minDistance)
            {

                float distance = Vector2.Distance(item.position, RecordPoints.First.Value);

                if (distance > minDistance)
                {

                    RecordPoints.AddFirst(item.position);

                }

            }

            public Vector2 DrawTracks()
            {

                LinkedListNode<Vector2> P = RecordPoints.First;

                while (P.Next != null)
                {

                    Debug.DrawLine(P.Value, P.Next.Value);

                    P = P.Next;

                }

                return P.Value;

            }

            public void RearFixed(float MaxDistance)
            {

                if (Distance > MaxDistance)
                {

                    LinkedListNode<Vector2> P = RecordPoints.Last;

                    while (P.Previous != null)
                    {

                        float distance = Vector2.Distance(P.Value, P.Previous.Value);

                        if (Distance - distance < MaxDistance)
                        {

                            P.Value = Vector2.Lerp(
                                P.Previous.Value,
                                P.Value,
                                (Distance - MaxDistance) / distance
                            );

                            break;

                        }
                        else
                        {

                            P = P.Previous;

                            RecordPoints.RemoveLast();

                        }

                    }

                }

            }

            public void Deliver(FollowItem Next, float MaxDistance, float MaxMoveLength)
            {

                float distance = Vector2.Distance(RecordPoints.Last.Value, Next.RecordPoints.First.Value);

                if (Distance + distance <= MaxDistance)
                {

                    return;

                }

                RecordPoints.AddLast(Next.RecordPoints.First.Value);

                LinkedListNode<Vector2> P = RecordPoints.Last;

                float LastMoveLength = MaxMoveLength;

                while (P.Previous != null)
                {

                    distance = Vector2.Distance(P.Value, P.Previous.Value);

                    if (Distance - distance <= MaxDistance)
                    {

                        Vector2 lerpPos = Vector2.Lerp(
                            P.Previous.Value,
                            P.Value,
                            (distance - Mathf.Max(LastMoveLength, Distance - MaxDistance)) / distance
                        );

                        Next.RecordPoints.AddFirst(lerpPos);

                        Next.item.position = Next.RecordPoints.First.Value;

                        RecordPoints.RemoveLast();

                        break;

                    }
                    else if (distance >= LastMoveLength)
                    {

                        Vector2 lerpPos = Vector2.Lerp(
                            P.Previous.Value,
                            P.Value,
                            (distance - LastMoveLength) / distance
                        );

                        Next.RecordPoints.AddFirst(lerpPos);

                        Next.item.position = Next.RecordPoints.First.Value;

                        RecordPoints.RemoveLast();

                        break;

                    }
                    else
                    {

                        LastMoveLength -= distance;

                        Next.RecordPoints.AddFirst(P.Value);

                        P = P.Previous;

                        RecordPoints.RemoveLast();

                    }

                }

            }

        }

        [SerializeField] private Transform FollowedObject;

        [SerializeField] private LinkedList<FollowItem> FollowItems;

        private void Start()
        {

            FollowItems = new LinkedList<FollowItem>();

            FollowItems.AddFirst(new FollowItem(FollowedObject));

        }

        public void AddItem(Transform newItem)
        {

            FollowItems.AddLast(new FollowItem(newItem));

            StartCoroutine(ChangeSpeed(FollowItems.Last.Value));

            newItem.GetComponent<IFollowable>().Follow(this);

        }

        public void RemoveItem(Transform removeItem)
        {

            LinkedListNode<FollowItem> P;

            for (P = FollowItems.First; P != null; P = P.Next)
            {

                if (P.Value.item == removeItem)
                {

                    if (P.Next != null)
                    {

                        LinkedListNode<FollowItem> next = P.Next;

                        for (LinkedListNode<Vector2> point = P.Value.RecordPoints.Last; point.Previous != null; point = point.Previous)
                        {

                            next.Value.RecordPoints.AddFirst(point.Value);

                        }

                    }

                    FollowItems.Remove(P);

                    break;

                }

            }

        }

        private IEnumerator ChangeSpeed(FollowItem item)
        {

            item.Speed = 0.1f;

            yield return new WaitForSeconds(0.5f);

            item.Speed = ItemMoveSpeed;

        }

        private void DrawRecordLine()
        {

            LinkedListNode<FollowItem> P = FollowItems.First;

            while (P.Next != null)
            {

                P.Value.DrawTracks();

                Debug.DrawLine(P.Value.RecordPoints.Last.Value, P.Next.Value.RecordPoints.First.Value);

                P = P.Next;

            }

            P.Value.DrawTracks();

        }

        private float RecordMinDistance = 10;

        private float ItemMaxDistance = 80.0f;

        private float ItemMoveSpeed = 10.0f;

        public void FixedUpdate()
        {

            FollowItems.First.Value.TryRecordPoint(RecordMinDistance);

            LinkedListNode<FollowItem> P = FollowItems.First;

            while (P.Next != null)
            {

                P.Value.Deliver(P.Next.Value, ItemMaxDistance, ItemMoveSpeed);

                P = P.Next;

            }

            FollowItems.Last.Value.RearFixed(ItemMaxDistance);

            //DrawRecordLine();

        }

        /// <summary>
        /// 尝试吃草莓
        /// </summary>
        /// <return>如果吃到了就返回true</return>
        public bool TryEatStrawberry()
        {

            foreach(var item in FollowItems)
            {

                if (item.item.TryGetComponent<Strawberry>(out Strawberry strawberry))
                {

                    strawberry.Eat();

                    return true;

                }

            }

            return false;

        }

        public void OnPlayerDead(n_Player.Player player)
        {

            FollowItem[] items = FollowItems.ToArray();

            foreach(var item in items) 
            {

                if (item.item.GetComponent<n_Player.PlayerEventInterface.IDead>() != null)
                {

                    item.item.GetComponent<n_Player.PlayerEventInterface.IDead>().OnPlayerDead(player);

                }

            }

        }

    }

}