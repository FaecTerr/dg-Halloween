using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class RussianSubtitles : Subtitles
    {
        public RussianSubtitles()
        {
            TimeOut = "Мир никогда не будет прежним";
            hunterWin = "Теперь мир в безопасности";
            firstMonologue = "Новый кошмар этого хэллоуина - Вы";
            LastGhost = "Все, кроме одного монстра убиты";

            PrePhaseEnded = "Охотники идут, заманите их в ловушки. Но самое главное - выживите";
            Remaining15 = "Осталось 15 секунд";
            Remaining30 = "Ещё 30 секунд";
            Remaining45 = "45 секунд осталось";

            GhostNear = "Они видят тебя";
            HunterNear = "Они здесь";

            ScannerActivate = "У тебя появилось время, чтобы убить";
            TrackerActivate = "Ты можешь бежать. Но тебе не спрятаться";
            Scanned = "Тебя обнаружили";
            Tracked = "За кем-то из вас следят";

            PMTrapDeployed = "Ловушка установлена";
            PMEnemyDied = "Противник погиб";
            PMTrapDestroyed = "Ловушка уничтожена";
            PMTimeIsOut = "Время вышло";
            PMInvis = "Невидимость включена";
            PMDetect = "Враг замечен";
            PMScan = "Сканирование активировано";
            PMTrack = "Отслеживание активировано";
            PMLantern = "Фонарь установлен";
            PMTrapActivated = "Ловушка сработала";

            PMBSilentKiller = "Скрытный убийца";
            PMBAvoidHunter = "Побег от охотника";

            HINTHammer = "Тесак на подобии молота, смертелен даже для тех, кто приоделся призраком";
            HINTChainsaw = "Легко перегревается, а раненные враги со временем истекают кровью";
            HINTSpear = "Позволяет совершить длинный рывок и оставаться невидимым, но замедляет передвижения";

            Hammer = "Тесак";
            Chainsaw = "Бензопила";
            Spear = "Когти хищника";

            HINTEDD = "Устанавливается на дверные проёмы и вызывает кровотечение врагу";
            EDD = "Растяжка";

            HINTGasSmoke = "Выпускает газ через время, после броска, который наносит урон и замедляет охотников";
            GasSmoke = "Баллон с газом";

            HINTClaymore = "Маленькая мина, которая заметна своими лазерами, если неприкрыта";
            Claymore = "Мина Клеймор";

            HINTKapkan = "Наносит урон пойманному мяснику, пока тот не умрёт или не будет освобождён товарищем";
            Kapkan = "Силок";

            HINTGrzmot = "После прилипания к поверхности, может ослепить врагов на время";
            Grzmot = "Оглушающая мина";

            Scan = "Сканер движения";
            HScan = "Если цель двинется, то она будет отображена";

            Tracker = "Анализатор следов";
            HTracker = "Раскрывает следы цели и отмечает самый свежий след в радиусе";

            Lamp = "Призрачный фонарь";
            HLamp = "Замедляет и отмечает цель, если та окажется слишком близко";

            Invis = "Бегство";
            HInvis = "Делает невидимым и быстрым на короткий промежуток времени";

            Challenge1Name = "День Джеймса Миллера";
            Challenge1Desc = "Убить 10 призраков молотом";

            Challenge2Name = "День Кожанного лица";
            Challenge2Desc = "Убить 10 призраков пилой";

            Challenge3Name = "День Фредди Крюгера";
            Challenge3Desc = "Убить 10 призраков когтями";

            Challenge4Name = "Смотри, куда идёшь";
            Challenge4Desc = "Сломать 50 ловушек";

            Challenge5Name = "Преследователь призраков";
            Challenge5Desc = "Обнаружить 25 призраков";

            Challenge6Name = "Мастер одного";
            Challenge6Desc = "5 раз поставьте в одном и том же раунде одинаковую ловушку";

            Challenge7Name = "Часики тикают";
            Challenge7Desc = "Выиграть 5 раундов за призрака временем";

            Challenge8Name = "Неуловимый";
            Challenge8Desc = "Сбежать от 10 охотников способностью Бегство";

            Challenge9Name = "На готове";
            Challenge9Desc = "Установить 50 ловушек";

            Challenge10Name = "День разнообразия";
            Challenge10Desc = "Поставить за раунд каждый вид ловушки (5 раз)";

            Challenge11Name = "Убитый убийца";
            Challenge11Desc = "Сделать 20 убийств ловушками";

            Challenge12Name = "День Каустика";
            Challenge12Desc = "Сделать 3 убийства газом";

            Challenge13Name = "Волки одиночки";
            Challenge13Desc = "Сделать 3 убийства силком";

            Challenge14Name = "Трюкач";
            Challenge14Desc = "Сделать убийство ослепляющей миной";

            Challenge15Name = "Восставшие из ада";
            Challenge15Desc = "Оживить все тыквы и выиграть раунд";

            Screenshake = "Тряска экрана";
            DictorVolume = "Гр.диктора";
            ShowHints = "Подсказки";
            ShowKeys = "Клавиши";
            ShowTimer = "Таймер";
            subt = "Субтитры";
            textLang = "Язык";
            exit = "ВЫХОД";

            ActionPhase = "Фаза действия";
            PreparationPhase = "Фаза подготовки";
            PlacableAdvice = "Удерживайте @SHOOT@ для установки";
            PlacableBad = "Недостаточно места";
            ThrowableAdvice = "Бросьте для активации";
            Coastline = "Побережье";
            Hospital = "Госпиталь";
            Prison = "Тюрьма";
            Vault = "Vault tec.";
            Westwood = "Вествуд";
            Mansion = "Поместье";
            Favela = "Фавелы";
            Nightcity = "Ночной город";
            Consulate = "Консульство";
            Factory = "Завод";
            Forest = "Лес";
            Arcade = "Аркада";
            Arctic = "Арктика";
        }
    }
}
