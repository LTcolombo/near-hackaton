using model;
using UnityEngine;
using utils.injection;
using view.behaviours.cell;

namespace view.behaviours.unit
{
    public class ShowTarget : InjectableBehaviour
    {
        [Inject]
        TargetModel _target;

        [Inject]
        FactionModel _faction;

        Transform _targetTransform;


        const float TWEEN_TIME = 1.5f;
        LineRenderer _line;
        private float _appearsFor;

        protected void Start()
        {
            _line = GetComponent<LineRenderer>();

            _target.Updated.Add(OnTargetUpdated);
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
                _appearsFor = 0;
                _targetTransform = target.transform;
            }
            else
            {
                _targetTransform = null;
            }
        }

        void Update()
        {
            if (_targetTransform == null)
            {
                _line.positionCount = 0;
                return;
            }

            _appearsFor += Time.deltaTime;
            if (_appearsFor < TWEEN_TIME)
            {
                _line.positionCount = 2;
                _line.SetPosition(0, transform.position);
                _line.SetPosition(1, _targetTransform.position);

                var color = ApplyFactionColor.FactionColors[_faction.Get(gameObject.GetInstanceID())];
                _line.startColor = new Color(color.r, color.g, color.b, (TWEEN_TIME - _appearsFor) / TWEEN_TIME);
                _line.endColor = new Color(color.r, color.g, color.b, (TWEEN_TIME - _appearsFor) / TWEEN_TIME);
            }
            else
                _line.positionCount = 0;
        }

        void OnDestroy()
        {
            _target.Updated.Remove(OnTargetUpdated);
        }
    }
}