using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class RagdollHandler : MonoBehaviour
    {
        private Rigidbody[] ragdollRigidbodies;
        private List<Collider> allColliders = new List<Collider>();
        public List<Collider> otherCollider = new List<Collider>();
        public Animator anim;
        public GameObject rootObject;
        [Header("KnockBack")]
        public float extraKnockBackUpValue = 0;
        public bool addKnockbackVelocity;
        public List<Rigidbody> ignoreKnockBackJoint;

        public float knockBackStrength = 0;
        public Vector3 knockBackDirection = Vector3.zero;




        [SerializeField] private List<Rigidbody> lessMovementColliderRoot = new List<Rigidbody>();
        public float lessMovementDrag = 0;
        public float lessMovementAngularDrag = 0;
        public float lessMovementMass = 0;
        private List<Rigidbody> currentLessMovementColliderRoot = new List<Rigidbody>();

        [HideInInspector] public UnityEvent onRagdollEnable = new UnityEvent();
        [HideInInspector] public UnityEvent onRagdollDisable = new UnityEvent();

        [SerializeField] private bool triggerRagdoll;

        private void Awake()
        {
            ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            allColliders.AddRange(GetComponentsInChildren<Collider>());

        }

        private void Update()
        {
            if (triggerRagdoll)
            {
                EnableRagdoll();
                triggerRagdoll = false;
            }
        }

        private void Start()
        {
            DisableRagdoll();
        }

        public void EnableRagdoll()
        {
            if (ragdollRigidbodies.Length <= 0)
                return;

            rootObject.transform.parent = null;

            anim.enabled = false;


            foreach (Collider c in otherCollider)
            {
                c.enabled = false;
            }

            foreach (Collider c in allColliders)
            {
                c.enabled = true;
            }

            foreach (Rigidbody rb in lessMovementColliderRoot)
            {
                currentLessMovementColliderRoot.AddRange(rb.gameObject.GetComponentsInChildren<Rigidbody>());
                currentLessMovementColliderRoot.Add(rb);
            }

            foreach (Rigidbody rb in currentLessMovementColliderRoot)
            {
                rb.mass = lessMovementMass;
                rb.linearDamping = lessMovementDrag;
                rb.angularDamping = lessMovementAngularDrag;
            }

            for (int i = 0; i < ragdollRigidbodies.Length; i++)
            {
                ragdollRigidbodies[i].isKinematic = true;
                ragdollRigidbodies[i].isKinematic = false;
                ragdollRigidbodies[i].useGravity = true;
                ragdollRigidbodies[i].linearVelocity = Vector3.zero;
                ragdollRigidbodies[i].interpolation = RigidbodyInterpolation.Interpolate;

                if (addKnockbackVelocity)
                {
                    if (ignoreKnockBackJoint.Contains(ragdollRigidbodies[i]))
                    {
                        ragdollRigidbodies[i].linearVelocity = Vector3.zero;
                        continue;
                    }
                    ragdollRigidbodies[i].AddForce(knockBackDirection * knockBackStrength +  (new Vector3(0, extraKnockBackUpValue, 0)) * knockBackStrength, ForceMode.Impulse);
                }
            }

            onRagdollEnable.Invoke();
            onRagdollDisable.Invoke();

        }

        public void DisableRagdoll()
        {
            if (ragdollRigidbodies.Length <= 0)
                return;


            foreach (Collider c in allColliders)
            {
                c.enabled = false;
            }

            foreach (Collider c in otherCollider)
            {
                c.enabled = true;
            }

            for (int i = 0; i < ragdollRigidbodies.Length; i++)
                ragdollRigidbodies[i].isKinematic = true;
        }
    }
}

