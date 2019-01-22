using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity.AI {
    public enum AIState { None, Patrol, Wander, Talking, Shop }
    public enum ShoppingDecision { None, Walk, Shop, Browse, Accept, Buy, Think, Talk }
    public enum TalkPattern { None, Agreeing, Talking }
    public enum AIAction { None, Interested, Curious, Yes, No, Think, Talk}
}