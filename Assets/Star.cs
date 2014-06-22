using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Star : MonoBehaviour {

	public List<Color> Colors;
	public float rate;
	bool isDead = false;

	public Color RandomColor;
	// Use this for initialization
	void Start () {
		RandomColor = Colors[Random.Range(0,Colors.Count-1)];

	}
	
	// Update is called once per frame
	void Update () {
		//this.particleSystem.startColor =  Color.Lerp(Color.white,RandomColor,Time.time/rate);

		ParticleSystem m_currentParticleEffect = (ParticleSystem)GetComponent("ParticleSystem");

		ParticleSystem.Particle []ParticleList = new    ParticleSystem.Particle[m_currentParticleEffect.particleCount];
		m_currentParticleEffect.GetParticles(ParticleList);
		for(int i = 0; i < ParticleList.Length; ++i)
		{
			float LifePercentage = (ParticleList[i].lifetime / ParticleList[i].startLifetime);
			ParticleList[i].color = Color.Lerp(Color.white, RandomColor, LifePercentage);
		}   
		
		m_currentParticleEffect.SetParticles(ParticleList, m_currentParticleEffect.particleCount);
	}
}
