# Платформа за онлајн учење

Оваа апликација престставува платформа за онлајн учење.
За апликацијата е користена .NET MVC технологија, додека за базата на податоци се користи PostgreSQL.

Во базата имаме податоци за сите регистрирани корисници (admins, instructors, students), категории (categories), курсеви (courses) заедно со своите видеа (videos) и текстуални документи (documents).
За секој студент се чуваат купените курсеви (bought_courses), гледаните курсеви (watched_courses), како и сертификати (certificates) за завршен одреден курс.

Исто така во базата чуваме и форуми (forums) каде што студентите и инструкторите можат да оставаат коментари за одредени теми.

* Повиците до базата од апликацијата се направени едноставни SELECT наредби, или CALL PROCEDURE повици.
