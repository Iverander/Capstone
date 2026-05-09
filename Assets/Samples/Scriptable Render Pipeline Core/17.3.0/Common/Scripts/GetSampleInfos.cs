using System.Collections;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class GetSampleInfos : MonoBehaviour
{
    public enum Type
    {
        Introduction,
        Title,
        Description
    }

    public Type type;
    public GameObject prefab;

    private TextMeshPro TextMeshProComponent;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0);
        UpdateText();
    }

    // Called when something has changed in the script
    private void OnValidate()
    {
        UpdateText();
    }

    private void UpdateTextMeshProReference()
    {
        TextMeshProComponent = GetComponent<TextMeshPro>();
        if (TextMeshProComponent == null)
            Debug.LogError($"TextMeshPro Component cannot be found on this GameObject: {gameObject.name}");
    }

    private void UpdateText()
    {
        if (TextMeshProComponent == null)
            UpdateTextMeshProReference();

        switch (type)
        {
            case Type.Introduction:
                TextMeshProComponent.text = SamplesShowcase.GetSanitizedIntroduction();
                break;
            case Type.Title:
                TextMeshProComponent.text = SamplesShowcase.GetSanitizedTitle(prefab.name);
                break;
            case Type.Description:
                TextMeshProComponent.text = SamplesShowcase.GetSanitizedDescription(prefab.name);
                break;
        }
    }
}