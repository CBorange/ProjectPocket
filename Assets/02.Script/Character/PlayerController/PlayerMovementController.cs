using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public FollowCamera followCamera;
    public PlayerAnimationController animationController;
    public PlayerStatManager statManager;

    public void HorizontalMovement(float inputVecX, float inputVecZ)
    {
        // 카메라가 바라보는 방향 계산
        Vector3 vec = transform.position - new Vector3(followCamera.transform.position.x, 0, followCamera.transform.position.z);
        Vector3 cameraViewDir = new Vector3(vec.x, 0, vec.z).normalized;

        // 월드 정면 기준으로 카메라가 얼마만큼 회전하였는지 계산
        float camAngle = CorrectAngle(MathUtil.GetAngleBetweenVecExceptY(cameraViewDir, Vector3.forward));
        Debug.Log(camAngle);

        // 카메라가 바라보는 방향이 회전한 각도만큼 입력벡터를 회전시킴
        Vector3 inputVec = new Vector3(inputVecX, 0, inputVecZ).normalized;
        Quaternion camRotateAngle = Quaternion.Euler(0, camAngle, 0);
        inputVec = camRotateAngle * inputVec;

        // 변환된 입력벡터가 월드 정면 기준으로 얼마만큼 회전되었는지 계산
        float inputVecAngle = CorrectAngle(MathUtil.GetAngleBetweenVecExceptY(inputVec, Vector3.forward));
        transform.rotation = Quaternion.Euler(0, inputVecAngle, 0);
    }
    private float CorrectAngle(float angle)
    {
        if (angle <= 90)
            angle *= -2;
        else if (angle <= 180)
            angle = 360 + (angle * -2);
        return angle;
    }
}
