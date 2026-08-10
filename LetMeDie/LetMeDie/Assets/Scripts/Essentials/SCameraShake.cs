using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

    public class SCameraShake : MonoBehaviour {

        public static SCameraShake Instance;

        public AnimationCurve falloffCurve = new(new []{new Keyframe(0,1, 0,0), new Keyframe(1, 0, -2,0)});
        private List<CameraShakeRequest> _requests = new();

        public CinemachineCamera CurrentlyUsedCamera;
        private CinemachineBasicMultiChannelPerlin noise;
        [SerializeField] private bool canShake = true;
        public bool CanShake {
            set => canShake = value;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private void Start() {
            noise = CurrentlyUsedCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        //Ampitude Gain = 1
        //Frquency Gain = 0.75
    }

        public void FixedUpdate() {

            if (!canShake)
            {
                return;
            }
            
            float intensity = 0;
            float frequency = 0;

            for (int i = _requests.Count - 1; i >= 0; i--) {
                float calculatedIntensity = _requests[i].Intensity * falloffCurve.Evaluate(1.0f - _requests[i].DeltaTime);
                if (calculatedIntensity > intensity) {
                    intensity = calculatedIntensity;
                    frequency = _requests[i].Frequency;
                }
                _requests[i].Tick(Time.deltaTime);
                if (!_requests[i].IsValid) {
                    _requests.RemoveAt(i);
                }
            }
            noise.AmplitudeGain = intensity;
            noise.FrequencyGain = frequency;
            
        }

    public void ChangeFOV(float fov)
    {
        CurrentlyUsedCamera.Lens.FieldOfView = fov;
    }

    public void ShakeForSeconds(float intensity, float frequency, float duration) {
            if (!canShake)
            {
                return;
            }
            CameraShakeRequest request = new(intensity, frequency, duration);
            if (request.IsValid) {
                _requests.Add(request);
            }
        }
    }

