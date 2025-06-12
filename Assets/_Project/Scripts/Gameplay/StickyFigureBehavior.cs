using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay
{
    public class StickyFigureBehavior : FigureSpecialBehavior
    {
        private List<FixedJoint2D> joints = new List<FixedJoint2D>();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out FigureView otherFigure) && otherFigure.BehaviorFigureConfig.figureType != FigureType.Sticky && joints.Count < 2)
            {
                FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
                joint.connectedBody = otherFigure.Rigidbody;
                joint.breakForce = 50f;

                joints.Add(joint);
            }
        }

        private void Update()
        {
            joints.RemoveAll(joint => joint == null);
        }
    }

}
