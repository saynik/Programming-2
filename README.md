# Space Invaders
                      
Máte před sebou práci studenta :

	1 ročníku fakulty MFF Univerzity Karlovy 
	
	2018-2019 akademický rok
	
	Saydametov Nikita

Tématem mé práce je hra Space Invaders. Tomohiro Nisikado vymyslel a vyvinul tuto hru ve formě herního arkádového automatu v roce 1978.  Mým úkolem bylo vytvoření obdoby této hry psané v jazyce C#.

## Popis Hry

Dle žánru Space Invaders je shoot 'em up, v níž hráč ovládá laserové dělo, které se pohybuje horizontálně ve spodní části obrazovky. Jeho úkolem je postupně zničit všechny mimozemské lodě. Cílem hry je postupně zničit všechny mimozemské lodě, které tvoři formaci jedenácti plavidel v pěti řadách a pohybují se vodorovně i horizontálně směrem k dolní části obrazovky.  Hráč má nekonečný počet nábojů. Za každou sestřelenou loď hráč obdrží určitý počet bodů. Se zničením mimozemšťanů se zvyšuje rychlost pohybu zbývajících a také se urychluje tempo zvukových efektů. Když jsou všichni mimozemšťané zničeni, objeví se nová, ještě silnější vlna a hráč dostane jeden život navíc. Počet nových vln mimozemšťanů je neomezený, což činí hru nekonečnou.

Navíc vesmírná plavidla útočníků nejsou bezbranná, ale mohou se aktivně bránit shazováním bomb na pohyblivé dělo. Čas od času prolétá ve vrchní části obrazovky speciální loď, jejíž zásah muže hráči přinést body navíc. Pokud se hráči podaří zneškodnit všechny přistávající lodě, začíná další kolo se stejným rozestavením, ale mnohem těžší obtížností. Laserové dělo je částečně chráněno několika defenzivní mi bunkry. Tyto bunkry mohou být zničeny hráčem a mimozemšťany. Hra končí v momentě, kdy hráč ztratí všechny své životy, nebo když se vetřelcům podaří dosáhnout povrchu planety a přistát.

## Download
Hotový projekt si můžete stáhnout na [tomto](https://github.com/saynik/Programming-2/releases) odkazu

## Instalace a spuštění hry

1) Chcete-li začít pracovat s touto hrou, musíte nainstalovat "Microsoft Visual Studio" (nejnovější verzi pro váš operační systém si můžete stáhnout z webových stránek [https://visualstudio.microsoft.com]() )

2) Dále budete potřebovat nainstalovaný Microsoft .NET Framework 4.6.1.
Developer Pack, který najdete ve složce Developer Pack, nebo jej stáhnete z webových stránek výrobce ([https://www.microsoft.com/enus/download/details.aspx?id=49978]())

3) Chcete-li spustit hru, je třeba spustit soubor SpaceInvaders.sln umístěný v kořenové složce. Poté se otevře Visual Studio, musíte stisknout "F5" a je to! Hra běží!

## Ovládání 

Ovládání hry probíhá na naprosto intuitivní úrovni, kde:

- Šipka doleva - pohyb laserového děla doleva

- Šipka doprava - pohyb laserového děla doprava

- Enter - vyber položky menu

## Komentáře ke hře (Technická část)

V této hře se používají algoritmy (ne personalizované). Většinou se používá lineární vyhledávání. Dále se používá procházení pole, aktualizace stavů objektů atd.

Kreslení prvků probíhá pomocí matrici (je pixel/není pixel) (tímto způsobem jsou nakresleny všechny objekty - mimozemšťané, dělo, speciální loď).

Aktualizace stavů výstřelů: podíváme se na výstřel od konce na začátek, zkontrolujeme, zda výstřel zasáhl cíl. Pokud ano, pak ho odstraníme ze seznamu a přejdeme k dalšímu (zeptáte se "Proč od konce na začátek, ne naopak?" a já vám odpovím "Nemusíte tak brát v úvahu změněnou velikost seznamu!") Pokud výstřel ještě musí letět, pak se jednoduše aktualizuje jeho souřadnice (+dy).

Chtěl bych se také zmínit o pohybu mimozemšťanů.  Pohybují se po spirále. Nejprve se pohybují na konec doleva, pak se posouvají dolů a mění směr svého pohybu na opačný.  A zase, jakmile se dosáhnou k pravému okraji, posunují se dolů a zase se pohybují doleva.

## Závěr

Na závěr bych chtěl říci, že pro mě byl tento projekt velmi zajímavým i kvůli tomu, že pro mě je to první zkušenost s vývojem hry !!! Osobně jsem velmi spokojen se svou prací, protože veškeré prvky, které jsem vložil do této práce fungují správně přesně tak, jak jsem zamýšlel! Každý ví, že nyní existuje obrovské množství her s moderní grafikou, chytlavými soundtracky, vzrušující hratelností, ale nesmíme zapomenout, že všechno v našem životě má začátek! A v našem případě hry jako Space Invaders, Pong, Pac-Man atd. jsou předchůdci současného herního průmyslu!

#  Děkuji Vám za pozornost k mé práci!
