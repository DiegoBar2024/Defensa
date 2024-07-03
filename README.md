PRIMERA ENTREGA
NOTA 1 :Referente al punto 1 de "Criterios de Calificación", decidimdos mantener la estructura lo mas feaciente al UML y CRC hecho por nosotros, sabemos que: deberiamos separar algunos metodos en clases para cumplir con los principios de SRP(CrearDepositos, CrearUsuarios, etc). Será corregido en próximas entregas.

NOTA 2: La no compilacion del código es directamente proporcional a que muchos metodos solo son estructuras, teniendolo en cuenta para futuras entregas.

NOTA 3: Los Unit test no fueron creados ya que prescindimos de dichos conocimientos.

SEGUNDA ENTREGA
Se comentará por clase los patrones y principos utilizados, detalles de funcionamiento y observaciones de código:

USO DE PRINCIPIOS Y PATRONES EN EL CÓDIGO

Clase Administrador: Clase que representa un Administrador y hereda de la clase de Usuario. Se decide implementar herencia siendo la clase Administrador la clase sucesora y Ususario la base a efectos de cubrir la responsabilidad de Administrador de dar de alta a un Usuario, para que este pueda usar la aplicación y/o poder administrarla. La herencia se decide hacer en este orden con el proposito de que el Administrador tenga más privilegios que el Usuario. Esta clase utiliza CREATOR para los siguientes metodos: CrearDeposito, CrearSección, AltaUsuario, AltarProveedor delegando responsabilidades de creación de objetos a otras clases. Al aplicar el patrón CREATOR en gran medida se entiende que se produce un aumento considerable en el acoplamiento de la clase Administrador, pero a su vez se hace a efectos de minimizar las responsabilidades que tiene el Administrador aumentando así la cohesión de la clase. De ésta manera, como principios utilizamos SRP para delegar responsabilidades a otras clases y EXPERT para el metodo de obtener lista de ventas para un determinado día.

Clases BuscadorDepositos y BuscadorSecciones: Utilziación del patrón POLIMORFISMO para las siguientes clases: BuscadorDepositos, BuscadorSecciones y IBuscador implementa una intefaz genérica. La operación polimórfica es Buscar y tiene una implementación distinta para las clases BuscadorSecciones y BuscadorDepositos.

Clase Usuario: Se utiliza el principio de SRP para delegar responsabilidades a otras clases (ejemplo: cuando se realiza una venta, se le delega la responsabildiad de disminuir el stock total a la clase VentaTotal y la responsabilidad de agregar una venta por fecha a la calase ContenedorVentasPorFechas). Como patron se utiliza CREATOR en el metodo AltaProducto, delegando la responsabilidad e instanciar un Producto a Seccion. Finalmente no se realiza el metodo de buscar el deposito más cercano debido a que en esta etapa aún no se cuenta con la API de Geolocalización que porporcionarán los docentes en la tercera entrega.

Clase Proveedor: Aplica el principio SRP ya que tiene unicamente la responsabilidad de visualizar stock de un determinado producto para tidos los depositos.

Clase Producto: Esta clase implementa la interfaz IProducto con el fin de aplicar el principio DIP ya que todas las clases que dependían directamente de Producto ahora dependerán de la abstracción IProducto. Por otro lado, mediante el hecho de que producto implemente la interfaz IProducto se aplica el principio OCP. Ésto se debe ya que las secciones ahora conocerán instancias IProducto en lugar de producto. De ésta manera cuando se quiera asignar la responsabilidad a una seccion de conocer otros elementos distintos de un producto, no será necesario alterar el codigo de la clase Seccion (fuera de agregar un método que se encargue de instanciar un objeto del tipo nuevo) y la extensión de responsabilidades consistirá unicamente en la implementación de un nuevo derivado de la abstracción IProducto.

Clase Seccion: Ésta clase implementa la interfaz ISeccion. De éste modo se utiliza el principio DIP ya que todas las clases que inicialmente dependían directamente de Seccion ahora dependerán de la abstracción ISeccion. Usando el patrón CREATOR, como se comentó anteriormente, la clase Usuario delega la responsabilidad de instanciar objetos del tipo IProducto a la sección. Debido que Seccion agrega objetos del tipo IProducto, resulta adecuado aplicar CREATOR en éste caso. Análogo a lo realizado con IProducto, al implementar la interfaz ISección se aplica el principio OCP ya que un depósito pasará a conocer unicamente objetos ISeccion y no seccion unicamente.

Clase VentaIndividual: La clase VentaIndividual va a delegar a la clase Sección la responsabilidad de modificar el stock del producto al realizar la venta individual. Se observa que la venta individual tendrá un stock de venta asociado, el código del producto vendido y una sección. Ésto contribuye en la distribución de responsabilidades entre distintas clases.

Clase VentaTotal: Usando el patrón CREATOR, como se comentó anteriormente, la clase Usuario delega la responsabilidad de instanciar objetos de la clase VentaIndividual a VentaTotal. Ésto se debe a que la clase VentaTotal contiene múltiples instancias de la clase VentaIndividual y por lo tanto resulta idónea para la creación de objetos VentaIndividual según CREATOR. Usando el patrón EXPERT, se delega la responsabilidad de disminuir el stock correspondiente a cada venta individual a la clase VentaIndividual, ya que ésta contiene la información suficiente ("clase experta") para realizar ésta funcionalidad. Ésto ayuda también a repartir responsabilidades entre las distintas clases.

Clase Deposito: Esta clase implementa la interfaz IDeposito con el fin de aplicar el principio OCP ya que todas las clases que dependían directamente de Deposito ahora pasarán a depender de la abstracción IDeposito. Por otro lado, usando el patrón CREATOR la clase Depósito es idónea para la creación de objetos del tipo ISeccion debido que Deposito contiene múltiples objetos de dicho tipo.

Clase ContenedorDepositos: Usando el patrón CREATOR, ésta clase es idónea para la creación de objetos del tipo IDeposito ya que la clase ContenedorDepositos contiene múltiples instancias del tipo IDeposito.

Clase ContenedorProveedores: Usando el patrón CREATOR, ésta clase es idónea para la creación de objetos del tipo Proveedor ya que la clase ContenedorProveedores contiene múltiples instancias del tipo Proveedor.

Clase ContenedorUsuarios: Usando el patrón CREATOR, ésta clase es idónea para la creación de objetos del tipo Usuario ya que la clase ContenedorUsuarios contiene múltiples instancias del tipo Usuario. Según los permisos especificados como entrada, el usuario puede ser de tipo Administrador (se recuerda que Administrador es un SUBTIPO de Usuario debido a la herencia explicada anteriormente).

Clase ContenedorVentasPorFecha: Usando el patrón EXPERT, se asigna la responsabilidad a ésta clase de realizar la búsqueda de todas las ventas en un determinado día. Se considera que es la clase experta en éste caso ya que la información necesaria para realizar ésta operación se encuentra contenida en un atributo de la clase (diccionario VentasPorFecha).

COSAS QUE SE PUEDEN MEJORAR EN EL PROGRAMA

Se podría aplicar en algunos casos el principio ISP para poder asegurar que las clases usen todos los métodos/propiedades de aquellas interfaces de las que usan operaciones. Por ejemplo, Administrador utiliza únicamente la operación CrearSeccion definida por IDeposito (no se usan las operaciones GetNombre ni GetSecciones) mientras que CreadorCategorias utiliza unicamente la operación GetSecciones definidas en IDeposito (no se usan las operaciones GetNombre ni CrearSeccion). La solución en éste caso constaría en dividir la interfaz IDeposito en dos (o más) interfaces distintas de modo que Deposito las implemente y que cada clase que usaba una operación de la interfaz IDeposito ahora utilice directamente las subinterfaces (no habría operaciones que no tengan uso). El caso de aplicar ISP también puede extenderse a otras interfaces del programa como ISeccion e IProducto.

DESAFÍOS

Se enfrentó a una modalidad de trabajo diferente a la acostumbrada en la carrera de Ingeniería en Telecomunicaciones. En concreto el uso de GitHub fue algo nuevo en conjunto con la complejidad en el estructuramiento del programa usando todos los patrones y principios vistos en el curso.


TERCERA ENTREGA 
----------------------------------------------------------------------------------------------
Se utiliza el patrón SINGLETON para las siguientes clases: BuscadorDepositos, BuscadorSecciones,  AdministradorHandler, UsuarioHandler y ProveedorHandler. Este patrón busca instanciar por única vez el objeto de una clase.

Por EXPERT asignamos la responsabilidad de obtener la cadena de los nombres de los depósitos a la clase ContenedorDepositos, ya que es la que conoce todos los depósitos que están creados.

*INICIO DE INTERACCIÓN CON EL BOT*
+ Para iniciar la conversación por Telegram se debe usar la palabra clave *hola*. 
Luego recibirá un mensaje de bienvenida. Posteriormente debe poner la palabra *administrador* para comenzar a interactuar (repitiendo la misma palabra para la contraseña).
+ Las excepciones se mostrarán luego de finalizar el formulario que corresponda a la opción elegida. Éstas mostrarán si fue tomado el formulario correctamente o debe hacerlo nuevamente.
+ Al seleccionar la opción *Salir*, se vuelve al menú principal, es decir, se debe iniciar sesión con el perfil deseado.
+ Para el cálculo de la distancia entre depósitos, la interacción con el usuario es más tardía que el resto.
