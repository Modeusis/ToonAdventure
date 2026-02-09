using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Core.Audio;
using Game.Scripts.Core.Loop;
using Game.Scripts.Utilities.Constants;

namespace Game.Scripts.Core.Scenes
{
    public class SceneLoader
    {
        private float _loadDelay;
        
        public SceneLoader(float fakeDelaySeconds)
        {
            _loadDelay = fakeDelaySeconds;
        }
        
        public async UniTask LoadGameplay()
        {
            G.UI.Loading.Show("Загружаем уровни...");
            
            G.UI.Screens.Menu.HideBackground();
            G.UI.Screens.Menu.Close();

            await UniTask.Delay(TimeSpan.FromSeconds(_loadDelay));
            
            await G.Loader.LoadSceneAsync(Addresses.BOOT_SCENE_KEY);
            await G.Loader.LoadSceneAsync(Addresses.GAMEPLAY_SCENE_KEY);

            G.UI.Screens.HUD.StaticTooltip.FadeIn();
            
            await G.Loader.InstantiateAsync<GameplayManager>(Addresses.GAMEPLAY_MANAGER_KEY);
        }
        
        public async UniTask LoadMain()
        {
            G.UI.Loading.Show("Загружаем меню...");
            
            G.UI.Screens.HUD.Hide();
            
            G.UI.Screens.Menu.ShowBackground();
            G.UI.Screens.Menu.Open();
            
            await UniTask.Delay(TimeSpan.FromSeconds(_loadDelay));
            
            await G.Loader.LoadSceneAsync(Addresses.BOOT_SCENE_KEY);
            await G.Loader.LoadSceneAsync(Addresses.MAIN_MENU_SCENE_KEY);
            
            G.Audio.PlayMusic(MusicType.Menu);
            G.UI.Loading.Hide();

            G.Cursor.Unlock();
        }
    }
}