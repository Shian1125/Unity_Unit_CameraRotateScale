using UnityEngine;
using System.Collections;
/// <summary>
/// 此腳本需掛在camera上
/// </summary>
public class CameraRotateScale : MonoBehaviour {

    public Transform target;            //攝影機目標
    public float xSpeed = 20;           //旋轉x軸速度
    public float ySpeed = 20;           //旋轉y軸速度
    public float yMinLimit = -50;       //y軸最小值
    public float yMaxLimit = 50;        //y軸最大值
    public float dis = 7;               //目標距離
    public float minDis = 4;            //最遠距離
    public float maxDis = 30;           //最近距離
    public float pinchSpeed = -.25f;    //縮放速度

    Touch touch;
    Vector3 angles;
    float curDist;      //兩指目前間距
    float lastDist;     //兩指最後間距
    float x;
    float y;
    // Use this for initialization
    void Start () {
        angles = this.transform.eulerAngles;
        x = angles.x;
        y = angles.y;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void LateUpdate() {
        if (target) {
            //旋轉
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
                touch = Input.GetTouch(0);
                x += touch.deltaPosition.x * xSpeed * 0.02f;
                y -= touch.deltaPosition.y * ySpeed * 0.02f;
                y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
                if (x < -360) x = 0;
                if (x >  360) x = 0;
                
            }
            //縮放
            if (Input.touchCount > 1 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)) {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                curDist = Vector2.Distance(touch1.position, touch2.position);
                if (curDist > lastDist) {
                    dis += Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition) * pinchSpeed / 10;
                } else {
                    dis -= Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition) * pinchSpeed / 10;
                }
                lastDist = curDist;
            }
            dis = Mathf.Clamp(dis, minDis, maxDis);
            Quaternion rota = Quaternion.Euler(y, x, 0);            //改變攝影機旋轉
            Vector3 disVector = new Vector3(0, 0, -dis);
            Vector3 posi = rota * disVector + target.position;      //改變攝影機位置
            this.transform.rotation = rota;
            this.transform.position = posi;
        }
        
    }
}
