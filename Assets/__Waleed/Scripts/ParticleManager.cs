using System;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    public ParticleContainer[] particleContainer;



    public bool isGrassVFX = false;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #region Play Particle

    #endregion

    #region SpwanParticle

    public void SpawnParticle(string name, Vector3 pos, float destroyDelay)
    {
        ParticleContainer particles = Array.Find(particleContainer, particle => particle.name == name);
        GameObject gb = particles.particle;
        var _particle = Instantiate(gb);
        _particle.transform.position = pos;
        Destroy(_particle.gameObject, destroyDelay);
    }

    public void SpawnParticle(string name, Vector3 pos, Material material, float destroyDelay = 2)
    {
        ParticleContainer particles = Array.Find(particleContainer, particle => particle.name == name);
        GameObject gb = particles.particle;
        var _particle = Instantiate(gb);
        _particle.transform.position = pos;

        ParticleSystem[] particleSystems = _particle.GetComponentsInChildren<ParticleSystem>();

        foreach (var item in particleSystems)
        {
            ParticleSystemRenderer systemRenderer = item.GetComponent<ParticleSystemRenderer>();
            systemRenderer.material = material;
        }

        Destroy(_particle.gameObject, destroyDelay);
    }





    #endregion

}

[System.Serializable]
public class ParticleContainer
{
    public string name;
    public GameObject particle;
}
