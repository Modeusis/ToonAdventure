// --- ГЛОБАЛЬНЫЕ ПЕРЕМЕННЫЕ ---
VAR level3_toys_found = 0

// --- DEBUG HUB: ДЛЯ ТЕСТОВ В INKY ---
// Чтобы проверить конкретный диалог, убери "//" перед стрелочкой
// -> level1_intro
// -> level1_leo_tutorial
// -> level1_balcony_toy
// -> level2_adam_dialogue
// -> level2_puzzle_monologue
// -> level2_puzzle_solved
// -> level3_maksim_start
// -> level3_toy_tree
// -> level3_toy_rock
// -> level3_mushrooms_finish

// Если ничего не выбрано, просто заглушка
-> END

// ============================================================
// ЛОКАЦИЯ 1: ПРИХОЖАЯ + БАЛКОН
// ============================================================

=== level1_intro ===
#speaker:storytelller #portrait:Storyteller
Шарль вернулся домой после долгого сезона. Тишина казалась подозрительной.
#speaker:storytelller #portrait:Storyteller
Вместо приветствия он обнаружил хаос: любимые вещи Лео были разбросаны повсюду.
#speaker:charlie #portrait:MainCharacter
(вздыхает) Ну вот, опять... Лео! Ты где?
#speaker:charlie #portrait:MainCharacter
Придется собрать все его игрушки, пока кто-нибудь не споткнулся.
-> END

=== level1_leo_tutorial ===
// Триггер: Игрок подходит к собаке в прихожей
#speaker:leo #portrait:Dog
Гав! Гав-гав!
#speaker:charlie #portrait:MainCharacter
Лео, мальчик, что ты натворил? И почему ты так смотришь на дверь балкона?
#speaker:charlie #portrait:MainCharacter
(Мысли) Нужно подойти к предмету и нажать кнопку взаимодействия, чтобы подобрать его.
#speaker:leo #portrait:Dog
Вуф! (убегает на балкон и исчезает за дверью)
-> END

=== level1_balcony_toy ===
// Триггер: Подбор первой игрушки на балконе
#speaker:charlie #portrait:MainCharacter
Ага! Вот и первая. Резиновая уточка... Серьезно, Лео?
#speaker:charlie #portrait:MainCharacter
Ладно, одной меньше. Дверь на кухню открыта, кажется, оттуда пахнет... горелым?
-> END

// ============================================================
// ЛОКАЦИЯ 2: КУХНЯ
// ============================================================

=== level2_adam_dialogue ===
// Триггер: Разговор с Адамом
#speaker:adam #portrait:Chief
О, Шарль! Ты уже здесь? Я... эээ... просто проверяю температуру духовки.
#speaker:charlie #portrait:MainCharacter
Адам? Почему ты в колпаке? И где Лео?
#speaker:adam #portrait:Chief
Этот пушистый гонщик пробегал здесь. Он засунул что-то ценное в холодильник.
#speaker:adam #portrait:Chief
Но вот беда — я не знаю код блокировки. Хотя лампочки мигают в странном порядке.
#speaker:adam #portrait:Chief
Посмотри на магниты-цифры. Цвета лампочек должны совпадать с цветом цифр.
#speaker:adam #portrait:Chief
Красный, синий... хмм... Разберись с этим, а у меня соус убегает!
-> END

=== level2_puzzle_monologue ===
// Триггер: Клик по холодильнику до решения
#speaker:charlie #portrait:MainCharacter
Заблокировано. Адам говорил про цвета.
#speaker:charlie #portrait:MainCharacter
Надо посмотреть, каким цифрам соответствуют цвета лампочек, и нажать их по порядку.
-> END

=== level2_puzzle_solved ===
// Триггер: Успешное решение загадки (открытие холодильника)
#speaker:charlie #portrait:MainCharacter
Есть! Открылся.
#speaker:charlie #portrait:MainCharacter
Боже, Лео... Замороженная косточка? Надеюсь, ты не собирался это есть.
-> END

=== level2_pickup_simple_toy ===
// Триггер: Подбор второй игрушки на кухне (на столе)
#speaker:charlie #portrait:MainCharacter
Плюшевый руль от симулятора. Лео явно хочет, чтобы я чаще был дома.
-> END

// ============================================================
// ЛОКАЦИЯ 3: УЧАСТОК У ДОМА (ОПУШКА)
// ============================================================

=== level3_maksim_start ===
// Триггер: Вход на локацию, Максим стоит у развилки
#speaker:maksim #portrait:Blonde
Привет, приятель. Решил прогуляться?
#speaker:charlie #portrait:MainCharacter
Макс? Ты что, охраняешь сад? Я ищу остатки игрушек Лео.
#speaker:maksim #portrait:Blonde
Видел твоего "штурмана". Он носился тут как угорелый.
#speaker:maksim #portrait:Blonde
Кажется, он выронил что-то у Большого Дерева, а потом побежал к Каменной Глыбе.
#speaker:maksim #portrait:Blonde
Посмотри там. Но будь осторожен на апексе, трава скользкая.
-> END

=== level3_toy_tree ===
// Триггер: Подбор игрушки у Дерева
~ level3_toys_found = level3_toys_found + 1
#speaker:charlie #portrait:MainCharacter
Нашел! Плюшевый бублик. Он застрял прямо в корнях.
-> check_leo_missing

=== level3_toy_rock ===
// Триггер: Подбор игрушки у Глыбы
~ level3_toys_found = level3_toys_found + 1
#speaker:charlie #portrait:MainCharacter
А вот и колокольчик. Лежал прямо на вершине камня. Как он туда забрался?
-> check_leo_missing

=== check_leo_missing ===
// Логическая проверка: Если собрали 2 игрушки на этом уровне
{ level3_toys_found == 2:
    -> level3_monologue_leo_missing
}
-> END

=== level3_monologue_leo_missing ===
// Автоматический монолог после сбора двух игрушек
#speaker:charlie #portrait:MainCharacter
Так, игрушки у меня. Макс говорил про дерево и камень... Я все проверил.
#speaker:charlie #portrait:MainCharacter
(оглядывается)
#speaker:charlie #portrait:MainCharacter
Но где сам Лео? Здесь слишком тихо.
#speaker:charlie #portrait:MainCharacter
Осталась только грибная тропинка в лесу. Может, он убежал туда?
-> END

=== level3_mushrooms_finish ===
// Триггер: Игрок заходит в зону грибной тропы (Финал)
#speaker:charlie #portrait:MainCharacter
Лео? Мальчик, ты здесь?
#speaker:leo #portrait:Dog
ВУФ! ВУФ!
#speaker:charlie #portrait:MainCharacter
Вот ты где! Ты напугал мен...
#speaker:storytelller #portrait:Storyteller
Шарль вышел на поляну и замер.
#speaker:maksim #portrait:Blonde
СЮРПРИЗ!
#speaker:adam #portrait:Chief
С возвращением домой, чемпион! Торт уже порезан!
#speaker:charlie #portrait:MainCharacter
Ребята... Вы все это устроили?
#speaker:leo #portrait:Dog
(Радостно виляет хвостом с мячиком в зубах)
#speaker:charlie #portrait:MainCharacter
(Смеется) Ладно, Лео. Игрушки твои. Но торт — мой!
-> END