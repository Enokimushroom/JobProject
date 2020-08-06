using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillMoveMode
{
    [Tooltip("跟踪并保持位置")]
    FollowRemain=1,
    [Tooltip("不跟踪并释放出去")]
    UnfollowRelease,

}
