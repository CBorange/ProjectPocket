using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region Singleton
    private static PlayerMovementController instance;
    public static PlayerMovementController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerMovementController>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerMovementController").AddComponent<PlayerMovementController>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PlayerMovementController>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    public FollowCamera followCamera;
    public Animator animator;
    public Rigidbody myRigidbody;
    public bool NowJumping;

    private void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 point = collision.contacts[0].point;
        if (point.y < transform.position.y + 0.5f) 
        {
            NowJumping = false;
            PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Idle;
            animator.speed = 1;
            animator.SetBool("FlyAir", false);
        }
    }
    public void HorizontalMovement(float moveVecX, float moveVecZ)
    {
        if (moveVecX == 0 && moveVecZ == 0)
        {
            PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Idle;
            animator.SetBool("Walk", false);
            return;
        }
        // 카메라가 바라보는 방향 계산
        Vector3 vec = transform.position - new Vector3(followCamera.transform.position.x, 0, followCamera.transform.position.z);
        Vector3 cameraViewDir = new Vector3(vec.x, 0, vec.z).normalized;

        // 월드 정면 기준으로 카메라가 얼마만큼 회전하였는지 계산
        float camAngle = CorrectAngleToSigned(MathUtil.GetAngleBetweenVecExceptY(cameraViewDir, Vector3.forward));

        // 카메라가 바라보는 방향이 회전한 각도만큼 이동벡터를 회전시킴
        Vector3 moveVec = new Vector3(moveVecX, 0, moveVecZ).normalized;
        Quaternion camRotateAngle = Quaternion.Euler(0, camAngle, 0);
        moveVec = camRotateAngle * moveVec;

        // 변환된 입력벡터가 월드 정면 기준으로 얼마만큼 회전되었는지 계산
        float inputVecAngle = CorrectAngleToSigned(MathUtil.GetAngleBetweenVecExceptY(moveVec, Vector3.forward));

        // 계산된 각도만큼 회전, 플레이어 이동
        transform.rotation = Quaternion.Euler(0, inputVecAngle, 0);
        transform.Translate(transform.forward * PlayerStat.Instance.MoveSpeed * Time.deltaTime, Space.World);
        PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Move;
        animator.speed = PlayerStat.Instance.MoveSpeed / 2;
        animator.SetBool("Walk", true);
    }
    public void Jump()
    {
        if (NowJumping)
        {
            return;
        }
        myRigidbody.AddForce(Vector3.up * PlayerStat.Instance.JumpSpeed, ForceMode.Impulse);
        NowJumping = true;
        PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Jump;
        animator.speed = 1;
        animator.SetTrigger("Jumped");
        animator.SetBool("FlyAir", true);
    }
    private float CorrectAngleToSigned(float angle)
    {
        if (angle <= 90)
            angle *= -2;
        else if (angle <= 180)
            angle = 360 + (angle * -2);
        return angle;
    }
}
