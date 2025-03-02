using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int numberOfGems { get; private set; }
    public int numberOfItems { get; private set; }

    private int totalGems;
    private int totalItems;

    public UnityEvent<PlayerInventory> OnGemCollected;
    public UnityEvent<PlayerInventory> OnItemCollected;
    public UnityEvent OnAllCollected;
    [SerializeField] private FinalDoorCinematic finalDoorCinematic;

    void Awake()
    {
        if (OnGemCollected == null)
            OnGemCollected = new UnityEvent<PlayerInventory>();

        if (OnItemCollected == null)
            OnItemCollected = new UnityEvent<PlayerInventory>();
        
        if (OnAllCollected == null)
            OnAllCollected = new UnityEvent();
    }

    void Start()
    {
        // Récupère le nombre total de gems et d'items dans la scène au début
        totalGems = Object.FindObjectsByType<Gem>(FindObjectsSortMode.None).Length;
        totalItems = Object.FindObjectsByType<ItemInteractable>(FindObjectsSortMode.None).Length;
    }
    

    public void CollectGem()
    {
        numberOfGems++;
        OnGemCollected.Invoke(this);
        CheckAllCollected();
    }

    public void CollectItem()
    {
        numberOfItems++;
        OnItemCollected.Invoke(this); // Informe l'UI ou d'autres scripts
        CheckAllCollected();
    }

    private void CheckAllCollected()
    {
        if (numberOfGems >= totalGems && numberOfItems >= totalItems)
        {
            // Ajouter un léger délai avant de lancer la cinématique pour éviter que l'item final ne bug
            Invoke(nameof(StartCinematic), 1f);
        }
    }

    private void StartCinematic()
    {
        finalDoorCinematic.PlayCinematic();
    }

}
