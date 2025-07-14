using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PokemonGO.Code
{
    [Serializable]
    public class PokeBallFactory
    {
        [SerializeField] private List<PokeballPrefab> _pokeballs;

        public PokeBall Create(PokeBallType type, Transform parent)
        {
            var pokeballPrefab = _pokeballs.First(p => p.Type == type).Prefab;
            return Object.Instantiate(pokeballPrefab, parent);
        }

        public PokeBall Create(Vector3 position, Quaternion rotation)
        {
            var pokeballPrefab = _pokeballs.First(p => p.Type == PokeBallType.Pokeball).Prefab;
            return Object.Instantiate(pokeballPrefab, position, rotation);
        }
    }

    [Serializable]
    public class PokeballPrefab
    {
        public PokeBallType Type;
        public PokeBall Prefab;
    }
}