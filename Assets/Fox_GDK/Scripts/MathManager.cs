/*
 * Developer Name: Md. Imran Hossain
 * E-mail: sandsoftimer@gmail.com
 * FB: https://www.facebook.com/md.imran.hossain.902
 * in: https://www.linkedin.com/in/md-imran-hossain-69768826/
 * 
 * This is a manager which will give common functional supports. 
 * like Math Calculator.
 *  
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.FunFox.Utility
{
    public class MathManager : MonoBehaviour
    {
        public float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }

        public Vector3 CenterOfTransform(Transform[] _transform)
        {
            Vector3[] vectors = new Vector3[_transform.Length];
            int index = 0;
            foreach (Transform item in _transform)
            {
                vectors[index++] = item.position;
            }
            return CenterOfVectors(vectors);
        }

        public Vector3 CenterOfVectors(Vector3[] vectors)
        {
            Vector3 sum = Vector3.zero;
            if (vectors == null || vectors.Length == 0)
            {
                return sum;
            }

            foreach (Vector3 vec in vectors)
            {
                sum += vec;
            }
            return sum / vectors.Length;
        }

        // Genarate Teajectory slots
        public Vector3[] GetParabolaPoints(Rigidbody rigidbody, Vector3 pos, Vector3 velocity, int steps)
        {
            Vector3[] results = new Vector3[steps];

            float timestep = Time.fixedDeltaTime / Physics.defaultSolverVelocityIterations;
            Vector3 gravityAccel = Physics.gravity * 1 * timestep * timestep;
            float drag = 1f - timestep * rigidbody.linearDamping;
            Vector3 moveStep = velocity * timestep;

            for (int i = 0; i < steps; ++i)
            {
                moveStep += gravityAccel;
                moveStep *= drag;
                pos += moveStep;
                results[i] = pos;
            }

            return results;
        }

        public Vector3[] GetTrajectoryPoints(Rigidbody rigidbody, Vector3 pos, Vector3 velocity, int layers, int steps = 2)
        {
            Vector3[] results = new Vector3[steps];
            List<Vector3> trajectoryPoints = new List<Vector3>();

            float timestep = Time.fixedDeltaTime / Physics.defaultSolverVelocityIterations;
            Vector3 gravityAccel = Physics.gravity * 1 * timestep * timestep;
            float drag = 1f - timestep * rigidbody.linearDamping;
            Vector3 moveStep = velocity * timestep;

            results[0] = pos;
            trajectoryPoints.Add(pos);
            for (int i = 1; i < steps; ++i)
            {
                moveStep += gravityAccel;
                moveStep *= drag;
                pos += moveStep;
                results[i] = pos;

                RaycastHit acol;
                Vector3 direction = results[i] - results[i - 1];
                Physics.Raycast(new Ray(results[i - 1], direction), out acol, direction.magnitude, layers);

                if (acol.collider == null)
                {
                    trajectoryPoints.Add(pos);
                }
                else
                {
                    if (acol.collider.gameObject.layer.Equals(ConstantManager.LAYER_GROUND))
                    {
                        trajectoryPoints.Add(acol.point);

                        Vector3 reflectVec = Vector3.Reflect(acol.point - rigidbody.position, acol.normal).normalized * velocity.magnitude / 4;
                        //ReflectionForce force = acol.collider.GetComponent<ReflectionForce>();
                        //reflectVec *= (force != null ? acol.collider.GetComponent<ReflectionForce>().force : 1);
                        //rforces.Add(reflectVec);
                        int availableSteps = 20;
                        Vector3[] reflectCurve = GetTrajectoryPoints(
                            rigidbody,
                            acol.point,
                            reflectVec,
                            availableSteps);

                        for (int j = 0; j < reflectCurve.Length; j++)
                        {
                            trajectoryPoints.Add(reflectCurve[j]);
                        }
                    }
                    break;
                }
            }

            return trajectoryPoints.ToArray();
        }

        public RaycastHit IsSomthingThere(Vector3 startPoint, Vector3 endPoint, int layers = 1)
        {
            Vector3 direction = endPoint - startPoint;
            RaycastHit rayOut;
            Physics.Raycast(new Ray(startPoint, direction), out rayOut, direction.magnitude, layers);
            return rayOut;
        }

        public Vector3 GetVelocityForThisPoint(Vector3 startPosition, Vector3 endPosition, float angle)
        {
            Vector3 direction = endPosition - startPosition;
            float h = direction.y;
            direction.y = 0;
            float distance = direction.magnitude;
            float a = angle * Mathf.Deg2Rad;
            direction.y = distance * Mathf.Tan(a);
            distance += h / Mathf.Tan(a);

            // calculate velocity
            float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            return velocity * direction.normalized;
        }

        public Vector3 GetForceForThisPoint(Vector3 startPosition, Vector3 endPosition, float angle, float mass)
        {
            return GetVelocityForThisPoint(startPosition, endPosition, angle) * mass / Time.fixedDeltaTime;
        }

        public Vector3 GetDirectionToPosition(Vector3 fromPosition)
        {
            Vector3 mouseWorldPosition = GetWorldTouchPosition(Vector3.up);
            return (mouseWorldPosition - fromPosition).normalized;
        }

        public Vector3 GetWorldTouchPosition(Vector3 axis)
        {
            Plane plane = new Plane(axis, 0);

            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }

            return Vector3.zero;
        }

        public Vector3 GetWorldTouchPosition(GameObject go)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1 << go.layer))
            {
                return hit.point;
            }

            return go.transform.position;
        }

        public float WrapAngle(float angle)
        {
            angle %= 360;
            return angle = angle > 180 ? angle - 360 : angle;
        }

        public float UnwrapAngle(float angle)
        {
            if (angle >= 0)
                return angle;

            angle = -angle % 360;

            return 360 - angle;
        }

        // Is Mouse over a UI Element? Used for ignoring World clicks through UI
        public bool IsPointerOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            else
            {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);
                return hits.Count > 0;
            }
        }

    }
}
