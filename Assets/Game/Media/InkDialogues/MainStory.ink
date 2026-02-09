// --- ГЛОБАЛЬНЫЕ ПЕРЕМЕННЫЕ ---
VAR level3_toys_found = 0

// --- DEBUG HUB: ДЛЯ ТЕСТОВ В INKY ---
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

// Заглушка, если не выбран дебаг
-> END

// ============================================================
// ЛОКАЦИЯ 1: ПРИХОЖАЯ + БАЛКОН
// ============================================================

=== level1_intro ===
#speaker:storytelller
Шарль вернулся домой после долгого сезона. Тишина казалась подозрительной.
#speaker:storytelller
Вместо приветствия он обнаружил хаос: любимые вещи Лео были разбросаны повсюду.
#speaker:charlie
(вздыхает) Ну вот, опять... Лео! Ты где?
#speaker:charlie
Придется собрать все его игрушки, пока кто-нибудь не споткнулся.
-> END

=== level1_leo_greatings ===
#speaker:leo
Гав! Гав-гав!
#speaker:charlie
Лео, мальчик, что ты натворил? Опять все собирать.
#speaker:leo
Вуф! (убегает к балкон и исчезает за поворотом)
-> END

=== level1_leo_runaway ===
#speaker:charlie
(Мысли) Надо догнать лео, и узнать зачем он убежал
-> END

=== level1_leo_near_balcony ===
#speaker:charlie
Лео, почему ты сюда, прибежал
#speaker:leo
Гав, гав!
#speaker:charlie
А, понял, пойду осмотрюсь на балконе
#speaker:charlie
(Мысли) Чтобы подобрать предмет нужно нажать кнопку взаимодействия
-> END

=== level1_balcony_toy ===
#speaker:charlie
Ага! Вот и первая. Резиновая уточка... Серьезно, Лео?
#speaker:charlie
Ладно, одной меньше. Нужно собрать остальные игрушки, пойду сначала на кухню.
-> END

=== level1_finished ===
#speaker:leo
Гав, гав, р-р-р!
#speaker:charlie
Ну ты молодец конечно, пойду смотреть что ты на кухне натворил
-> END

=== level1_blocked_exit ===
#speaker:charlie
(Мысли) Мне нужно сначала собрать все игрушки в прихожей перед тем как идти на кухню
-> END

=== level1_blocked_balcony ===
#speaker:charlie
(Мысли) На балконе, точно что-то не так, но мне нужно найти лео
-> END

// ============================================================
// ЛОКАЦИЯ 2: КУХНЯ
// ============================================================

=== level2_adam_dialogue ===
#speaker:adam
О, Шарль! Ты уже здесь? Я... эээ... просто проверяю температуру духовки.
#speaker:charlie
Адам? Почему ты в колпаке? И где Лео?
#speaker:adam
Этот пушистый гонщик пробегал здесь. Он засунул что-то ценное в холодильник.
#speaker:adam
Но вот беда — я не знаю код блокировки. Хотя лампочки мигают в странном порядке.
#speaker:adam
Посмотри на магниты-цифры. Цвета лампочек должны совпадать с цветом цифр.
#speaker:adam
Красный, синий... хмм... Разберись с этим, а у меня соус убегает!
-> END

=== level2_puzzle_monologue ===
#speaker:charlie
Заблокировано. Адам говорил про цвета.
#speaker:charlie
Надо посмотреть, каким цифрам соответствуют цвета лампочек, и нажать их по порядку.
-> END

=== level2_puzzle_solved ===
#speaker:charlie
Есть! Открылся.
#speaker:charlie
Боже, Лео... Замороженная косточка? Надеюсь, ты не собирался это есть.
-> END

=== level2_pickup_simple_toy ===
#speaker:charlie
Плюшевый руль от симулятора. Лео явно хочет, чтобы я чаще был дома.
-> END

// ============================================================
// ЛОКАЦИЯ 3: УЧАСТОК У ДОМА (ОПУШКА)
// ============================================================

=== level3_maksim_start ===
#speaker:maksim
Привет, приятель. Решил прогуляться?
#speaker:charlie
Макс? Ты что, охраняешь сад? Я ищу остатки игрушек Лео.
#speaker:maksim
Видел твоего "штурмана". Он носился тут как угорелый.
#speaker:maksim
Кажется, он выронил что-то у Большого Дерева, а потом побежал к Каменной Глыбе.
#speaker:maksim
Посмотри там. Но будь осторожен на апексе, трава скользкая.
-> END

=== level3_toy_tree ===
~ level3_toys_found = level3_toys_found + 1
#speaker:charlie
Нашел! Плюшевый бублик. Он застрял прямо в корнях.
-> check_leo_missing

=== level3_toy_rock ===
~ level3_toys_found = level3_toys_found + 1
#speaker:charlie
А вот и колокольчик. Лежал прямо на вершине камня. Как он туда забрался?
-> check_leo_missing

=== check_leo_missing ===
{ level3_toys_found == 2:
    -> level3_monologue_leo_missing
}
-> END

=== level3_monologue_leo_missing ===
#speaker:charlie
Так, игрушки у меня. Макс говорил про дерево и камень... Я все проверил.
#speaker:charlie
(оглядывается)
#speaker:charlie
Но где сам Лео? Здесь слишком тихо.
#speaker:charlie
Осталась только грибная тропинка в лесу. Может, он убежал туда?
-> END

=== level3_mushrooms_finish ===
#speaker:charlie
Лео? Мальчик, ты здесь?
#speaker:leo
ВУФ! ВУФ!
#speaker:charlie
Вот ты где! Ты напугал мен...
#speaker:storytelller
Шарль вышел на поляну и замер.
#speaker:maksim
СЮРПРИЗ!
#speaker:adam
С возвращением домой, чемпион! Торт уже порезан!
#speaker:charlie
Ребята... Вы все это устроили?
#speaker:leo
(Радостно виляет хвостом с мячиком в зубах)
#speaker:charlie
(Смеется) Ладно, Лео. Игрушки твои. Но торт — мой!
-> END