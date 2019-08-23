using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerButtonPrompts : MonoBehaviour {


	public enum PlayerActions
	{
		Dashing = 0,
		Jumping = 1,
		FiringGun = 2,
        Interact = 3,
        SwitchCompanion = 4
    }


	[System.Serializable]
	public enum ControllerType
	{
		Keyboard = 0,
		Xbox = 1,
		SNES = 2

	}


	[System.Serializable]
	public class ControllerMapping
	{
		public ControllerType controllerType;
		public Sprite DashButton;
		public Sprite JumpButton;
		public Sprite FireButton;
        public Sprite InteractButton;
        public Sprite SwitchCompanion;
    }


    public List<ControllerMapping> _listControllerMappings;

    private ControllerMapping _currentController;

    public Sprite ReturnCorrectButtonSprite(PlayerActions playerAction)
	{
		_currentController = getCurrentController();
        if (playerAction == PlayerActions.Dashing)
        {
            return _currentController.DashButton;
        }
        else if (playerAction == PlayerActions.Jumping)
        {
            return _currentController.JumpButton;
        }
        else if (playerAction == PlayerActions.FiringGun)
        {
            return _currentController.FireButton;
        }
        else if (playerAction == PlayerActions.Interact)
        {
            return _currentController.InteractButton;
        }
        else if (playerAction == PlayerActions.SwitchCompanion)
        {
            return _currentController.SwitchCompanion;
        }

        return _currentController.JumpButton;
	}


	private ControllerMapping getCurrentController()
	{
        if(_currentController == null)
        {
            return _listControllerMappings[2];
        }

		return _currentController;
	}


    public void setCurrentController(ControllerType type)
    {
        _currentController = _listControllerMappings.Where(x => x.controllerType == type).SingleOrDefault();
    }

}
