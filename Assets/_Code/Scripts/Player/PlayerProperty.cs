using UnityEngine;

public class PlayerProperty : MonoBehaviour
{
    public SkinnedMeshRenderer m_CharacterMeshRenderer;

    public Material m_Material;


    private void Start()
    {
        if (m_CharacterMeshRenderer)
            m_CharacterMeshRenderer.material = m_Material;
    }

}
