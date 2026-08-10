
    internal sealed class CameraShakeRequest {
        public float Intensity { get; private set; }
        public float Frequency { get; private set; }
        public float RemainingTime { get; private set; }
        public float Duration { get; private set; }

        public float DeltaTime => RemainingTime / Duration;

        public bool IsValid => Duration > 0 && RemainingTime > 0;

        public CameraShakeRequest(float intensity, float frequency, float duration) {
            Intensity = intensity;
            Frequency = frequency;
            Duration = duration;
            RemainingTime = duration;
        }

        public void Tick(float deltaTime) {
            RemainingTime -= deltaTime;
        }

        public void End() {
            RemainingTime = 0;
        }
    }
