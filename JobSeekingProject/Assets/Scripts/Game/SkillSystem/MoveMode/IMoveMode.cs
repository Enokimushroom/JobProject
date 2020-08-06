using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移动方法接口
/// </summary>
public interface IMoveMode
{
    void Excute(Deployer deployer);
}
