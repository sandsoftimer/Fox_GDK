using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FOVEffect : FoxObject
{
	[SerializeField] private float viewRadius;
	[SerializeField] private float viewAngle;
	[SerializeField] private Material mat;

	public LayerMask targetLayer;
	public LayerMask obstacleLayer;

	public bool targetOnView;

	[HideInInspector] public List<Transform> visibleTargets = new List<Transform>();

	public int angleResolution;
	public int edgeResolveIterations;
	public float edgeDstThreshold;

	MeshFilter viewMeshFilter;
	Mesh viewMesh;
	Vector3[] currentMeshVertices;

	public Color green = new Color(0f, 1f, 0.5f, 0.35f);
	private Color red = new Color(1f, 0f, 0.5f, 0.35f);

	public override void Start()
	{
		base.Start();

		viewMeshFilter = GetComponent<MeshFilter>();
		viewMesh = new Mesh { name = "View Mesh" };
        viewMeshFilter.mesh = viewMesh;
		mat.color = green;
	}

	void Update()
	{
		DrawFieldOfView();
		FindVisibleTargets();
	}
	
	void FindVisibleTargets()
    {
		if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
			return;

		for (int i = 0; i < currentMeshVertices.Length; i++)
        {
			Vector3 direction = transform.TransformPoint(currentMeshVertices[i]) - transform.position;
			targetOnView = Physics.Raycast(transform.position, direction, direction.magnitude, targetLayer);
			if (targetOnView)
            {
				gameState = GameState.GAME_PLAY_ENDED;
				gameplayData.gameoverSuccess = false;
				foxManager.ChangeGameState(GameState.GAME_PLAY_ENDED);
				mat.color = red;
				break;
            }
		}
		//for (int i = 0; i < currentMeshVertices.Length; i++)
		//{
		//	Debug.DrawLine(transform.position, transform.TransformPoint(currentMeshVertices[i]), Color.blue);
		//}
		mat.color = targetOnView ? red : green;
	}

	void DrawFieldOfView()
	{
		float stepAngleSize = viewAngle / (float)angleResolution;
		List<Vector3> viewPoints = new List<Vector3>();
		ViewCastInfo oldViewCast = new ViewCastInfo();
		for (int i = 0; i <= angleResolution; i++)
		{
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast(angle);

			if (i > 0)
			{
				bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
				if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
				{
					EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
					if (edge.pointA != Vector3.zero)
					{
						viewPoints.Add(edge.pointA);
					}
					if (edge.pointB != Vector3.zero)
					{
						viewPoints.Add(edge.pointB);
					}
				}
			}
			viewPoints.Add(newViewCast.point);
			oldViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count + 1;
		currentMeshVertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		currentMeshVertices[0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++)
		{
			currentMeshVertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
			if (i < vertexCount - 2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear();
		viewMesh.vertices = currentMeshVertices;
		viewMesh.triangles = triangles;
	}

    public override void OnGameOver()
    {
        base.OnGameOver();

        if (gameplayData.gameoverSuccess)
        {
			mat.color = green;
        }
        else
        {
			mat.color = red;
        }
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
	{
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < edgeResolveIterations; i++)
		{
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast(angle);

			bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
			if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
			{
				minAngle = angle;
				minPoint = newViewCast.point;
			}
			else
			{
				maxAngle = angle;
				maxPoint = newViewCast.point;
			}
		}

		return new EdgeInfo(minPoint, maxPoint);
	}

	ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleLayer))
		{
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
		{
			hit = _hit;
			point = _point;
			dst = _dst;
			angle = _angle;
		}
	}

	public struct EdgeInfo
	{
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
		{
			pointA = _pointA;
			pointB = _pointB;
		}
	}
}
