using UnityEngine;
using UnityEngine.UI;

public class BuildSelectButton : MonoBehaviour
{
    [SerializeField]
    public int AssignedBuildingNumber = -1;

    [SerializeField]
    public BuildingSO BuildingSO;

    private Button Button;
    private Image BackgroundImage;
    private bool Buildable = true;

    public void Awake()
    {
        Button = GetComponent<Button>();
        BackgroundImage = GetComponent<Image>();
        Button.onClick.AddListener(SelectBuilding);
    }

    private void Update()
    {
        if (AssignedBuildingNumber == -1 || BuildingSO == null)
        {
            Button.enabled = false;
            return;
        }
        BackgroundImage.sprite = BuildingSO.Sprite;

        if (!InventoryManager.Instance.HasEnoughItems(BuildingSO.Cost))
        {
            ColorBlock colorBlock = Button.colors;
            colorBlock.disabledColor = Color.red;
            Button.colors = colorBlock;
            Button.interactable = false;
            Buildable = false;
        }
        else
        {
            ColorBlock colorBlock = Button.colors;
            colorBlock.highlightedColor = Color.white;
            Button.colors = colorBlock;
            Button.interactable = true;
            Buildable = true;
        }

        Button.enabled = true;
    }

    private void SelectBuilding()
    {
        if (!Buildable)
        {
            //TODO: FEEDBACK FOR USER
            return;
        }
        BuildManager.Instance.SetSelectedBuilding(AssignedBuildingNumber);
    }
}