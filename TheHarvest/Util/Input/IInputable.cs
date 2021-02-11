namespace TheHarvest.Util.Input
{
    /*
    UI components will implement this interface, though will not 'execute' any
    interactions via InputInteract() - they will be interacted with regardless,
    due to how Nez handles UI components, though they will still check if input
    will be allowed to trickle down to lower priority components
    */
    public interface IInputable
    {
        // true if input collides with or is relevant to this component
        // or allows input to fall through to lower priority components
        bool InputCollision();
    }
}