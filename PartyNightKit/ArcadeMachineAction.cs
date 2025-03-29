using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TinyLife;
using TinyLife.Actions;
using TinyLife.Objects;
using TinyLife.Utilities;
public class ArcadeMachineAction : MultiAction {

    public ArcadeMachineAction(ActionType type, ActionInfo info) : base(type, info) {}

    protected override CompletionType AndThenIsCompleted() {
        // we want to complete our action once 10 minutes of sitting time have passed
        return this.CompleteIfTimeUp(System.TimeSpan.FromMinutes(10));
    }

    protected override IEnumerable<Action> CreateFirstActions()
    {
        var spot = this.Info.ToFreeActionSpot();
        if(spot != null) {
            // we want to move to the arcade machine
            yield return ActionType.GoHere.Construct<Action>(spot);
        }
    }
    protected override void AndThenUpdate(GameTime time, System.TimeSpan passedInGame, float speedMultiplier)
    {
        base.AndThenUpdate(time, passedInGame, speedMultiplier);
        this.Person.CurrentPose = this.Person.CurrentPose.ToWorking();
        this.Person.RestoreNeed(NeedType.Entertainment, 30, this.Info, speedMultiplier);
    }

    protected override void AndThenInitialize()
    {
        base.AndThenInitialize();
        var arcade = this.Info.GetActionObject<ScreenObject>();
        if(arcade != null) {
            arcade.TurnOn("Game");
        }
    }

    protected override void AndThenOnCompleted(CompletionType type)
    {
        base.AndThenOnCompleted(type);
        var arcade = this.Info.GetActionObject<ScreenObject>();
        if(arcade != null) {
            arcade.TurnOff();
        }
    }
}