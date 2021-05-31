Práctica de programación 
Laura del Villar Aparicio Latorre

--------------
Para cambiar entre almacenamiento persistente o volátil hace falta cambiar en el fichero Web.config 
el valor de almacenamiento dentro de appSettings. El valor será volatil o persistente según se desee.

Para el almacenamiento volátil he empleado sesiones en las que se guarda el listado de usuarios.

Para el almacenamiento persistente he utilizado la tecnología Entity Framework con la tabla previamente 
creada en Microsoft SQL Server Management Studio. 