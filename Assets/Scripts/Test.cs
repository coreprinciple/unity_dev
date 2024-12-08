using Common;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class Quest
{
    public Either<string, int> CompleteQuest(bool success)
    {
        if (success)
            return Either<string, int>.FromRight(100);
        else
            return Either<string, int>.FromLeft("Quest failed");
    }
}

public class Item
{
    public string Name { get; private set; }
}

public class InventorySlot
{
    public Optional<Item> Item { get; private set; } = Optional<Item>.None();

    Optional<int> durability = Optional<int>.Some(100);

    public void AddItem(Item item)
    {
        Item = Optional<Item>.Some(item);
    }

    public void DebugItem()
    {
        Debug.Log(
            Item.SelectMany(i =>
                durability.Select(d => $"{i.Name} (Durability: {d})")
            ).Match(
                summary => summary,
                () => "Item or durability missing"
            )
        );
    }

    public void UseItem(System.Action<Item> onItemPresent, System.Action onItemAbsent)
    {
        Item.Match(
            onValue: item => { onItemPresent(item); return 0; },
            onNoValue: () => { onItemAbsent(); return 0; }
        );
    }
}