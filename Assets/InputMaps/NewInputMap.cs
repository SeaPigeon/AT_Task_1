// GENERATED AUTOMATICALLY FROM 'Assets/InputMaps/NewInputMap.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @NewInputMap : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @NewInputMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""NewInputMap"",
    ""maps"": [
        {
            ""name"": ""UI"",
            ""id"": ""f29d149f-fbf8-422a-8fa3-7374edd8d52a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""036e57a1-d1c4-4d24-b3c2-9267d29bf0ef"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonSouth"",
                    ""type"": ""PassThrough"",
                    ""id"": ""96fb1c97-5da5-4ec9-a1bc-a8f3bb23965e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonEast"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4ca54080-cd64-495d-b1b8-5e557a28f36c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonNorth"",
                    ""type"": ""PassThrough"",
                    ""id"": ""da58a516-4cbd-4673-acd1-d219bd32db0e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonWest"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3efdce85-51ce-4475-a34e-c350b7dd1e19"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShoulderL"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b5789293-065a-452c-80fa-9d791511a425"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShoulderR"",
                    ""type"": ""PassThrough"",
                    ""id"": ""76783697-3142-44c8-9607-83c4e38b13b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StartButton"",
                    ""type"": ""PassThrough"",
                    ""id"": ""851b6b5a-a2ff-4c8d-a598-0078f429c48c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""2953e56f-021a-4a50-9227-838bb73356c5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""496cfed4-4ea7-4035-96ca-ee20882e0f5e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bb764479-ad0c-4d0a-94a5-7bbf5e52e609"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""254c7c66-e772-4122-b5ab-423d35471e5a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a276867f-bf65-4af6-b99c-37c137ce3cc2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""23fb3de6-c7f3-48d0-9810-812486d9ceda"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""deb2cc9b-2812-4c74-921c-adc275ef891b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f09b283-c6c0-4349-8cfe-6d5b31ca04d0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""851b0f73-d3c1-4567-8d12-83d6cf330483"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24292044-5a02-4bb0-853e-1ed4146936f8"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16c3e1d8-fdc9-40a0-a6fa-e98c714b91b7"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b05f813d-0c8d-47cd-a453-ded4d62e45da"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShoulderL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30c74648-afdd-4516-8001-0d48118ddb16"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShoulderL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25fce7fc-6ed5-4bb6-a5c7-c32431c94257"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShoulderR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4dad814e-56fe-4f23-a431-f52a22878105"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShoulderR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4f7bc52-b7bc-4924-83fa-efa0f4dc1f34"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00841910-b774-48d1-9265-8f41b82ab97e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Game"",
            ""id"": ""3a26d6e4-f975-40a8-a8ca-ebf742efaadd"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""6c7e1c5a-7219-42a1-b741-caeaacb6cea9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonSouth"",
                    ""type"": ""Button"",
                    ""id"": ""83dd792a-6dca-4b16-be47-0042be365640"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonEast"",
                    ""type"": ""Button"",
                    ""id"": ""b2ddb183-ebec-4163-9982-9c2a0a6c8c15"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonNorth"",
                    ""type"": ""Button"",
                    ""id"": ""88d05ca4-3d11-428f-9509-c46af3279bb8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonWest"",
                    ""type"": ""Button"",
                    ""id"": ""cfc50486-7d8e-488f-9739-d5b2ee7d2e83"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StartButton"",
                    ""type"": ""Button"",
                    ""id"": ""9bed6e47-dda3-4c25-bf8c-79b33d2cb47e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShoulderL"",
                    ""type"": ""Button"",
                    ""id"": ""fa2fba1d-5485-492a-9e4e-4a86c7a2fb6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShoulderR"",
                    ""type"": ""Button"",
                    ""id"": ""17ab006c-e9cc-487f-ba04-32034227b43b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a3aa5ae3-bfcf-4888-8e8e-f4a966ebba20"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1fa74d6d-9515-4b3b-b837-c5d3e4e586bf"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""722388d1-2b83-43e6-8b68-9381d397014e"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae42d9bf-bf4c-4c81-824d-ea2ee4e10e5f"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb7711c1-8964-4307-80a3-92549ea8747f"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73f9a43e-c843-423a-853f-635912c933b5"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04ea9559-0672-412c-a9e4-b56938686042"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99683022-b7bd-4b83-9f4d-fd3f4109e9ec"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5299f4d5-899d-469c-8fbf-52156bd67c25"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShoulderL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7cee4fdb-387e-4ac1-841e-090a68d51887"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShoulderL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d56e4fea-dde2-4fcc-9ee1-358f0c872a50"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShoulderR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a73870d2-f1cf-40b2-9d81-0416726147ae"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShoulderR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""8e7dd0ae-41a5-4e23-b983-850a31b1223d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f88c1c4f-eb6d-479c-8acc-056c9df37f75"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""77d980f2-4ef9-4041-a0be-9085826862a3"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""024f820d-65fc-4b7d-9f9d-6dba54b5cd66"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c26e006b-465b-42fd-abc0-4e564149abce"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ec23a7a7-864d-46aa-8f81-af74b7bd347e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Move = m_UI.FindAction("Move", throwIfNotFound: true);
        m_UI_ButtonSouth = m_UI.FindAction("ButtonSouth", throwIfNotFound: true);
        m_UI_ButtonEast = m_UI.FindAction("ButtonEast", throwIfNotFound: true);
        m_UI_ButtonNorth = m_UI.FindAction("ButtonNorth", throwIfNotFound: true);
        m_UI_ButtonWest = m_UI.FindAction("ButtonWest", throwIfNotFound: true);
        m_UI_ShoulderL = m_UI.FindAction("ShoulderL", throwIfNotFound: true);
        m_UI_ShoulderR = m_UI.FindAction("ShoulderR", throwIfNotFound: true);
        m_UI_StartButton = m_UI.FindAction("StartButton", throwIfNotFound: true);
        // Game
        m_Game = asset.FindActionMap("Game", throwIfNotFound: true);
        m_Game_Move = m_Game.FindAction("Move", throwIfNotFound: true);
        m_Game_ButtonSouth = m_Game.FindAction("ButtonSouth", throwIfNotFound: true);
        m_Game_ButtonEast = m_Game.FindAction("ButtonEast", throwIfNotFound: true);
        m_Game_ButtonNorth = m_Game.FindAction("ButtonNorth", throwIfNotFound: true);
        m_Game_ButtonWest = m_Game.FindAction("ButtonWest", throwIfNotFound: true);
        m_Game_StartButton = m_Game.FindAction("StartButton", throwIfNotFound: true);
        m_Game_ShoulderL = m_Game.FindAction("ShoulderL", throwIfNotFound: true);
        m_Game_ShoulderR = m_Game.FindAction("ShoulderR", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Move;
    private readonly InputAction m_UI_ButtonSouth;
    private readonly InputAction m_UI_ButtonEast;
    private readonly InputAction m_UI_ButtonNorth;
    private readonly InputAction m_UI_ButtonWest;
    private readonly InputAction m_UI_ShoulderL;
    private readonly InputAction m_UI_ShoulderR;
    private readonly InputAction m_UI_StartButton;
    public struct UIActions
    {
        private @NewInputMap m_Wrapper;
        public UIActions(@NewInputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_UI_Move;
        public InputAction @ButtonSouth => m_Wrapper.m_UI_ButtonSouth;
        public InputAction @ButtonEast => m_Wrapper.m_UI_ButtonEast;
        public InputAction @ButtonNorth => m_Wrapper.m_UI_ButtonNorth;
        public InputAction @ButtonWest => m_Wrapper.m_UI_ButtonWest;
        public InputAction @ShoulderL => m_Wrapper.m_UI_ShoulderL;
        public InputAction @ShoulderR => m_Wrapper.m_UI_ShoulderR;
        public InputAction @StartButton => m_Wrapper.m_UI_StartButton;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                @ButtonSouth.started -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonSouth;
                @ButtonSouth.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonSouth;
                @ButtonSouth.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonSouth;
                @ButtonEast.started -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonEast;
                @ButtonEast.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonEast;
                @ButtonEast.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonEast;
                @ButtonNorth.started -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonNorth;
                @ButtonNorth.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonNorth;
                @ButtonNorth.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonNorth;
                @ButtonWest.started -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonWest;
                @ButtonWest.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonWest;
                @ButtonWest.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnButtonWest;
                @ShoulderL.started -= m_Wrapper.m_UIActionsCallbackInterface.OnShoulderL;
                @ShoulderL.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnShoulderL;
                @ShoulderL.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnShoulderL;
                @ShoulderR.started -= m_Wrapper.m_UIActionsCallbackInterface.OnShoulderR;
                @ShoulderR.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnShoulderR;
                @ShoulderR.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnShoulderR;
                @StartButton.started -= m_Wrapper.m_UIActionsCallbackInterface.OnStartButton;
                @StartButton.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnStartButton;
                @StartButton.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnStartButton;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @ButtonSouth.started += instance.OnButtonSouth;
                @ButtonSouth.performed += instance.OnButtonSouth;
                @ButtonSouth.canceled += instance.OnButtonSouth;
                @ButtonEast.started += instance.OnButtonEast;
                @ButtonEast.performed += instance.OnButtonEast;
                @ButtonEast.canceled += instance.OnButtonEast;
                @ButtonNorth.started += instance.OnButtonNorth;
                @ButtonNorth.performed += instance.OnButtonNorth;
                @ButtonNorth.canceled += instance.OnButtonNorth;
                @ButtonWest.started += instance.OnButtonWest;
                @ButtonWest.performed += instance.OnButtonWest;
                @ButtonWest.canceled += instance.OnButtonWest;
                @ShoulderL.started += instance.OnShoulderL;
                @ShoulderL.performed += instance.OnShoulderL;
                @ShoulderL.canceled += instance.OnShoulderL;
                @ShoulderR.started += instance.OnShoulderR;
                @ShoulderR.performed += instance.OnShoulderR;
                @ShoulderR.canceled += instance.OnShoulderR;
                @StartButton.started += instance.OnStartButton;
                @StartButton.performed += instance.OnStartButton;
                @StartButton.canceled += instance.OnStartButton;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // Game
    private readonly InputActionMap m_Game;
    private IGameActions m_GameActionsCallbackInterface;
    private readonly InputAction m_Game_Move;
    private readonly InputAction m_Game_ButtonSouth;
    private readonly InputAction m_Game_ButtonEast;
    private readonly InputAction m_Game_ButtonNorth;
    private readonly InputAction m_Game_ButtonWest;
    private readonly InputAction m_Game_StartButton;
    private readonly InputAction m_Game_ShoulderL;
    private readonly InputAction m_Game_ShoulderR;
    public struct GameActions
    {
        private @NewInputMap m_Wrapper;
        public GameActions(@NewInputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Game_Move;
        public InputAction @ButtonSouth => m_Wrapper.m_Game_ButtonSouth;
        public InputAction @ButtonEast => m_Wrapper.m_Game_ButtonEast;
        public InputAction @ButtonNorth => m_Wrapper.m_Game_ButtonNorth;
        public InputAction @ButtonWest => m_Wrapper.m_Game_ButtonWest;
        public InputAction @StartButton => m_Wrapper.m_Game_StartButton;
        public InputAction @ShoulderL => m_Wrapper.m_Game_ShoulderL;
        public InputAction @ShoulderR => m_Wrapper.m_Game_ShoulderR;
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnMove;
                @ButtonSouth.started -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonSouth;
                @ButtonSouth.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonSouth;
                @ButtonSouth.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonSouth;
                @ButtonEast.started -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonEast;
                @ButtonEast.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonEast;
                @ButtonEast.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonEast;
                @ButtonNorth.started -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonNorth;
                @ButtonNorth.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonNorth;
                @ButtonNorth.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonNorth;
                @ButtonWest.started -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonWest;
                @ButtonWest.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonWest;
                @ButtonWest.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnButtonWest;
                @StartButton.started -= m_Wrapper.m_GameActionsCallbackInterface.OnStartButton;
                @StartButton.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnStartButton;
                @StartButton.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnStartButton;
                @ShoulderL.started -= m_Wrapper.m_GameActionsCallbackInterface.OnShoulderL;
                @ShoulderL.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnShoulderL;
                @ShoulderL.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnShoulderL;
                @ShoulderR.started -= m_Wrapper.m_GameActionsCallbackInterface.OnShoulderR;
                @ShoulderR.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnShoulderR;
                @ShoulderR.canceled -= m_Wrapper.m_GameActionsCallbackInterface.OnShoulderR;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @ButtonSouth.started += instance.OnButtonSouth;
                @ButtonSouth.performed += instance.OnButtonSouth;
                @ButtonSouth.canceled += instance.OnButtonSouth;
                @ButtonEast.started += instance.OnButtonEast;
                @ButtonEast.performed += instance.OnButtonEast;
                @ButtonEast.canceled += instance.OnButtonEast;
                @ButtonNorth.started += instance.OnButtonNorth;
                @ButtonNorth.performed += instance.OnButtonNorth;
                @ButtonNorth.canceled += instance.OnButtonNorth;
                @ButtonWest.started += instance.OnButtonWest;
                @ButtonWest.performed += instance.OnButtonWest;
                @ButtonWest.canceled += instance.OnButtonWest;
                @StartButton.started += instance.OnStartButton;
                @StartButton.performed += instance.OnStartButton;
                @StartButton.canceled += instance.OnStartButton;
                @ShoulderL.started += instance.OnShoulderL;
                @ShoulderL.performed += instance.OnShoulderL;
                @ShoulderL.canceled += instance.OnShoulderL;
                @ShoulderR.started += instance.OnShoulderR;
                @ShoulderR.performed += instance.OnShoulderR;
                @ShoulderR.canceled += instance.OnShoulderR;
            }
        }
    }
    public GameActions @Game => new GameActions(this);
    public interface IUIActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnButtonSouth(InputAction.CallbackContext context);
        void OnButtonEast(InputAction.CallbackContext context);
        void OnButtonNorth(InputAction.CallbackContext context);
        void OnButtonWest(InputAction.CallbackContext context);
        void OnShoulderL(InputAction.CallbackContext context);
        void OnShoulderR(InputAction.CallbackContext context);
        void OnStartButton(InputAction.CallbackContext context);
    }
    public interface IGameActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnButtonSouth(InputAction.CallbackContext context);
        void OnButtonEast(InputAction.CallbackContext context);
        void OnButtonNorth(InputAction.CallbackContext context);
        void OnButtonWest(InputAction.CallbackContext context);
        void OnStartButton(InputAction.CallbackContext context);
        void OnShoulderL(InputAction.CallbackContext context);
        void OnShoulderR(InputAction.CallbackContext context);
    }
}
