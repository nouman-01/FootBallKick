using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        float xroat, yroat = 0f;
        public float rotatespeed;
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        public bool isMove;
        public GameObject start;
        public GameObject ball;


        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                
            }
            
        }

        void Update()
        {
            //pathCreator.bezierPath.SetPoint(0, transform.localPosition);
            //var bezierpath = pathCreator.path.localPoints[0];
            //var bezierpath2 = pathCreator.path.localPoints[2];
            pathCreator.path.localPoints[0] = transform.localPosition;


            if (Input.GetMouseButton(0))
            {
                
                //ball.transform.localPosition = pathCreator.path.localPoints[0];
                //pathCreator.path.localPoints[0] = ball.transform.position;
                //Debug.Log(bezierpath);
            }
            if (Input.GetMouseButtonUp(0))
            {
                isMove = true;
            }
            if (isMove)
            {
                //if (pathCreator != null)
                //{
                //    distanceTravelled += speed * Time.deltaTime;
                //    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                //    transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                //}
            }

        }
        private void OnCollisionEnter(Collision collision)
        {
           
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "start")
            {
                Debug.Log("stop");
                isMove = false;
            }
        }
        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
    
}