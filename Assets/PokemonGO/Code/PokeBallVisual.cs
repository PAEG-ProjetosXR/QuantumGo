using UnityEngine;

namespace PokemonGO.Code
{
    [RequireComponent(typeof(PokeBall))]
    public sealed class PokeBallVisual : MonoBehaviour
    {
        [SerializeField] private PokeBall _pokeBall;
        [SerializeField] private ParticleSystem _chargedVfx;
        [SerializeField] private ParticleSystem _starsVfx;
        
        private void OnEnable()
        {
            _pokeBall.OnCharged += OnCharged;
            _pokeBall.OnDischarged += OnDischarged;
        }

        private void OnDisable()
        {
            _pokeBall.OnCharged -= OnCharged;
            _pokeBall.OnDischarged -= OnDischarged;
        }

        private void OnCharged()
        {
            _chargedVfx.Play(true);
            _starsVfx.Play(true);
        }

        private void OnDischarged()
        {
            _chargedVfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _starsVfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}