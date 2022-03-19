using System;
using System.Collections;
using model;
using utils.injection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace view.behaviours.unit
{
    public class MoveToTarget : InjectableBehaviour
    {
        [Inject]
        MoveParamsModel _moveParams;

        [Inject]
        TargetModel _target;

        Transform _targetTransform;
        Vector3 _fallbackPos;

        int _sign = 1;
        float _angle;

        protected void Start()
        {
            _target.Updated.Add(OnTargetUpdated);
            StartCoroutine(Variate());

            _fallbackPos = transform.position;
        }

        void OnTargetUpdated(int id)
        {
            if (gameObject.GetInstanceID() != id)
            {
                return;
            }

            var target = _target.GetTarget(gameObject.GetInstanceID());
            if (target != null)
            {
                _targetTransform = target.transform;
                _fallbackPos = _targetTransform.position;
            }
            else
            {
                _targetTransform = null;
            }
        }

        void Update()
        {
            if (_moveParams.Velocity <= 0)
                return;

            //rotate
            var targetTransformPosition = _fallbackPos;
            if (_targetTransform != null)
                targetTransformPosition = _targetTransform.position.normalized * 11f;

            var deltaPos = targetTransformPosition - transform.position;
            var targetProjection = Vector3.ProjectOnPlane(deltaPos, transform.position);

            var inertia = Random.Range(_moveParams.Inertia.RangeStart, _moveParams.Inertia.RangeEnd);

            var currentAngle = Vector3.SignedAngle(
                Vector3.ProjectOnPlane(Vector3.up,
                    transform.position), //after LookAt transform.up is set to Vector.up
                transform.up, transform.forward);

            var deltaAngle = 0f;

            switch (_moveParams.Type)
            {
                case MovementType.Wander:

                    //move
                    transform.Translate(transform.up.normalized * Time.deltaTime * _moveParams.Velocity, Space.World);

                    deltaAngle = Vector3.Angle(transform.up, targetProjection) * Time.deltaTime * _sign * 60 / inertia;
                    break;
                case MovementType.Direct:

                    //move
                    transform.Translate(transform.up.normalized * Time.deltaTime * _moveParams.Velocity, Space.World);

                    deltaAngle = Vector3.SignedAngle(transform.up, targetProjection, transform.forward);

                    if (deltaPos.magnitude > 2f) //todo magic numbers, apply to WANDER too.
                        deltaAngle /= inertia;
                    break;
                case MovementType.Harvest:

                    var targetOffset = -deltaPos;

                    if (Math.Abs(_harvestAngleProgress) < 1)
                    {
                        _harvestAngle = _harvestAngleProgress = Random.Range(60, 180) * _sign;
                    }

                    targetOffset = targetOffset.normalized * Mathf.Lerp(
                                       _moveParams.Inertia.RangeStart,
                                       _moveParams.Inertia.RangeEnd,
                                       (float) Math.Sin((_harvestAngle - _harvestAngleProgress) * Math.PI /
                                                        _harvestAngle));

                    var harvestAngleDelta = _harvestAngleProgress * Time.deltaTime * _moveParams.Velocity;
                    _harvestAngleProgress -= harvestAngleDelta;
                    targetOffset = Quaternion.AngleAxis(harvestAngleDelta, targetTransformPosition) * targetOffset;
                    //4. apply to position (target + delta)
                    transform.position = targetTransformPosition + targetOffset;

                    deltaAngle = Vector3.SignedAngle(transform.up, targetProjection, transform.forward);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            transform.position = transform.position.normalized * 11f; //put back on the orbit
            transform.LookAt(Vector3.zero);
            transform.Rotate(Vector3.forward, currentAngle + deltaAngle);
        }

        private float _harvestAngle = 25;
        private float _harvestAngleProgress = 5;

        private IEnumerator Variate()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(5, 15) / 10f);
                _sign *= -1;
            }
        }

        void OnDestroy()
        {
            _target.Updated.Remove(OnTargetUpdated);
        }
    }
}