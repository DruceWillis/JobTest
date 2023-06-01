using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthOrbSpawner
{
    private HealthOrb _healthOrbPrefab;
    private List<HealthOrb> _healthOrbs;
    
    public HealthOrbSpawner(HealthOrb healthOrbPrefab)
    {
        _healthOrbPrefab = healthOrbPrefab;
        _healthOrbs = new List<HealthOrb>();
        GameStateController.Instance.OnGameStateChanged += state =>
        {
            if (state == eGameState.Fighting || state == eGameState.MainMenu)
            {
                _healthOrbs.ForEach(ho => ho.gameObject.SetActive(false));
            }
        };
    }

    public void SpawnHealthOrb(Vector3 spawnPosition)
    {
        var orb = _healthOrbs.FirstOrDefault(ho => !ho.gameObject.activeSelf);
        
        if (orb)
        {
            orb.transform.position = spawnPosition;
        }
        else
        {
            orb = GameObject.Instantiate(_healthOrbPrefab, spawnPosition, Quaternion.identity);
            _healthOrbs.Add(orb);
        }
        orb.LaunchOrb();
    }
}