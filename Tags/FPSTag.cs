using System.Collections;
using TMPro;
using UnityEngine;

namespace FPSNametagsForZlothy.Tags;

public class FPSTag : MonoBehaviour
{
    private GameObject firstPersonTag;
    private GameObject thirdPersonTag;
    
    private TextMeshPro firstPersonTagText;
    private TextMeshPro thirdPersonTagText;
    
    private VRRig rig;

    private IEnumerator DelayedStart()
    {
        while (GetComponent<Nametag>() == null)
            yield return null;
        
        CreateNametags();
    }

    private void CreateNametags()
    {
        CreateNametag(ref firstPersonTag, ref firstPersonTagText, "FirstPersonTag", "FirstPersonOnly", false);
        CreateNametag(ref thirdPersonTag, ref thirdPersonTagText, "ThirdPersonTag", "MirrorOnly", true);
    }

    private void CreateNametag(ref GameObject tagObj, ref TextMeshPro tagText, string name, string layerName, bool isThirdPerson)
    {
        tagObj = new GameObject(name);
        tagObj.transform.SetParent(isThirdPerson ? GetComponent<Nametag>().thirdPersonTag.transform : GetComponent<Nametag>().firstPersonTag.transform);
        tagObj.transform.localPosition = new Vector3(0f, 0.2f, 0f);
        
        tagObj.layer = LayerMask.NameToLayer(layerName);
        
        tagText = tagObj.AddComponent<TextMeshPro>();
        tagText.fontSize = 2f;
        tagText.alignment = TextAlignmentOptions.Center;
        tagText.font = Plugin.comicSans;
        tagText.font.material.shader = Shader.Find("TextMeshPro/Distance Field");
    }

    private void Update()
    {
        if (rig == null)
            rig = GetComponent<VRRig>();
        
        int fps = rig.fps;
        
        Color tagColour = Color.green;

        switch (fps)
        {
            case < 60:
                tagColour = Color.red;
                break;
            case < 90:
                tagColour = Color.yellow;
                break;
            default:
                tagColour = Color.green;
                break;
        }
        
        firstPersonTagText.text = fps.ToString();
        thirdPersonTagText.text = fps.ToString();
        
        firstPersonTagText.color = tagColour;
        thirdPersonTagText.color = tagColour;
    }
    
    private void Start() => StartCoroutine(DelayedStart());
}