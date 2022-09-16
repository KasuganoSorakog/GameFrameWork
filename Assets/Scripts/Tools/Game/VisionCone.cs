using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 扇形检测
/// </summary>

public class VisionCone : MonoBehaviour
{
    Player player;

    [Header("Vision")]
    public float vision_angle = 30f;
    public float vision_range = 5f;
    public LayerMask obstacle_mask = ~(0);

    [Header("Material")]
    public Material cone_material;
    public Material cone_far_material;
    public int sort_order = 1;

    [Header("Optimization")]
    public int precision = 60;
    public float refresh_rate = 0f;

    private MeshRenderer render;
    private MeshFilter mesh;
    private float timer = 0f;

    //射线检测到的东西信息
    RaycastHit hit; 
    //检测到东西之后执行(每帧)
    public Action OnDetectedAction;
    //没检测到东西时候的执行(每帧)
    public Action OnDetected_NotAction;

    private void Awake()
    {
        render = gameObject.AddComponent<MeshRenderer>();
        mesh = gameObject.AddComponent<MeshFilter>();
        render.sharedMaterial = cone_material;
        render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        render.receiveShadows = false;
        render.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        render.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        render.allowOcclusionWhenDynamic = false;
        render.sortingOrder = sort_order;
    }

    //需要在管理器的Start中调用(或者在Player的初始化Init()里面调用)
    public void OnInit(Player player, Action detectedAction = null, Action detected_notAction = null)
    {
        this.player = player;
        InitMesh(mesh, false);
        OnDetectedAction = detectedAction;
        OnDetected_NotAction = detected_notAction;
    }

    //初始化检测网格(用于查看)
    private void InitMesh(MeshFilter mesh, bool far)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();

        if (!far)
        {
            vertices.Add(new Vector3(0f, 0f, 0f));
            normals.Add(Vector3.up);
            uv.Add(Vector2.zero);
        }

        int minmax = Mathf.RoundToInt(vision_angle / 2f);

        int tri_index = 0;
        float step_jump = Mathf.Clamp(vision_angle / precision, 0.01f, minmax);

        for (float i = -minmax; i <= minmax; i += step_jump)
        {
            float angle = (float)(i + 90f) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angle) * vision_range, 0f, Mathf.Sin(angle) * vision_range);

            vertices.Add(dir);
            normals.Add(Vector2.up);
            uv.Add(Vector2.zero);

            if (far)
            {
                vertices.Add(dir);
                normals.Add(Vector2.up);
                uv.Add(Vector2.zero);
            }

            if (tri_index > 0)
            {
                if (far)
                {
                    triangles.Add(tri_index);
                    triangles.Add(tri_index + 1);
                    triangles.Add(tri_index - 2);

                    triangles.Add(tri_index - 2);
                    triangles.Add(tri_index + 1);
                    triangles.Add(tri_index - 1);
                }
                else
                {
                    triangles.Add(0);
                    triangles.Add(tri_index + 1);
                    triangles.Add(tri_index);
                }
            }
            tri_index += far ? 2 : 1;
        }

        mesh.mesh.vertices = vertices.ToArray();
        mesh.mesh.triangles = triangles.ToArray();
        mesh.mesh.normals = normals.ToArray();
        mesh.mesh.uv = uv.ToArray();
    }

    //Tips: 需要在管理的Update中调用
    public void OnUpdate()
    {
        timer += Time.deltaTime;      
        if (timer > refresh_rate)
        {
            timer = 0f;           
            //第一层检测
            UpdateMainLevel(mesh, vision_range);
        }
    }

    private void UpdateMainLevel(MeshFilter mesh, float range)
    {
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(new Vector3(0f, 0f, 0f));
       
        int minmax = Mathf.RoundToInt(vision_angle / 2f);
        float step_jump = Mathf.Clamp(vision_angle / precision, 0.01f, minmax);

      
        //是否有检测到东西
        bool isTrigger = false;

        //------------检测到后执行的事件------
        //if (player.IsJoystickInput()) return;

        for (float i = -minmax; i <= minmax; i += step_jump)
        {
            float angle = (i + 90f) * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angle) * range, 0f, Mathf.Sin(angle) * range);

 
            Vector3 pos_world = transform.TransformPoint(Vector3.zero);
            Vector3 dir_world = transform.TransformDirection(dir.normalized);
            bool ishit = Physics.Raycast(new Ray(pos_world, dir_world), out hit, range, obstacle_mask.value);

            if (ishit)
            {
                isTrigger = true;
            }
            else
            {
                isTrigger = false;
            }
            vertices.Add(dir);
        }
        
        //判断是否检测到东西
        if (isTrigger)
        {
            OnDetected();
        }
        else
        {
            OnDetected_Not();
        }

        mesh.mesh.vertices = vertices.ToArray();
        mesh.mesh.RecalculateBounds();
    }


    //在此处写入检测方法
    private void OnDetected()
    {
        OnDetectedAction?.Invoke();
        //if (hit.collider.CompareTag(TagType.Seaweed))
        //{
        //    player.ShowWeapon(WeaponType.JuZi);
        //}
        //else if (hit.collider.CompareTag(TagType.Coral))
        //{
        //    player.ShowWeapon(WeaponType.JuZi);
        //}
        //else if (hit.collider.CompareTag(TagType.Ore))
        //{
        //    player.ShowWeapon(WeaponType.GaoTou);
        //}
        //else if (hit.collider.CompareTag(TagType.Enemy) && DataManager.Instance.playerData.isUnLockDao)
        //{
        //    player.ShowWeapon(WeaponType.Dao);
        //}
        //else if ((hit.collider.CompareTag(TagType.WreckShip) || hit.collider.CompareTag(TagType.WreakSubmarine)) && DataManager.Instance.playerData.isUnLockPick)
        //{
        //    player.ShowWeapon(WeaponType.GaoTou);
        //}
        //else
        //{
        //    isTrigger = false;
        //}
        //if (player.curState.Equals(PlayerState.Idle))
        //{
        //    player.ChangeState(PlayerState.Collect);
        //}       
    }

    //当没有检测到任何东西,且当前状态是采集状态时候(分两种情况，检测是否有摇杆输入，如果有就是奔跑状态)
    private void OnDetected_Not()
    {
        OnDetected_NotAction?.Invoke();
        //if (player.IsJoystickInput())
        //{
        //    player.ChangeState(PlayerState.Swimming);
        //    if (player.player_Weapon)
        //    {
        //        player.player_Weapon.SetActive(false);
        //        player.SetWeaponATK(false);
        //        player.player_Weapon = null;
        //    }
        //}
        //else if(player.IsAtkAnimEnd())
        //{
        //    player.ChangeState(PlayerState.Idle);
        //    if (player.player_Weapon)
        //    {
        //        player.player_Weapon.SetActive(false);
        //        player.SetWeaponATK(false);
        //        player.player_Weapon = null;
        //    }
        //}
        //else if (player.IsPlayerIdle())
        //{
        //    if (player.player_Weapon)
        //    {
        //        player.player_Weapon.SetActive(false);
        //        player.SetWeaponATK(false);
        //        player.player_Weapon = null;
        //    }
        //}
    }
}



