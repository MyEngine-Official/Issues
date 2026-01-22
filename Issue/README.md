# Issue - To-Do App (.NET MAUI)

Esta solución contiene una app de tareas en .NET MAUI enfocada en Android. Se implementan pantallas, ViewModels y servicios para manejar tareas, notificaciones y alarmas.

## Estructura

- **Models**: `TaskItem`, `NotificationOffset`.
- **Services**: `TaskService`, `NotificationService`, `IAlarmService`, `INotificationManager`.
- **ViewModels**: `TaskListViewModel`, `TaskDetailViewModel`.
- **Views**: `TaskListPage`, `TaskDetailPage`, `NotificationSettingsPage`.
- **Android**: `AlarmService`, `AlarmReceiver`, `AlarmStopReceiver`.

## Flujo principal

1. **Lista de tareas**: `TaskListPage` muestra las tareas en un `CollectionView`.
2. **Crear/Editar**: botón "Nueva" abre `TaskDetailPage` para crear o editar.
3. **Notificaciones**: el servicio `NotificationService` genera hasta 3 horarios con offset.
4. **Alarmas**: `AlarmService` usa `AlarmManager` y `AlarmReceiver` dispara la notificación y el sonido.

## Notas

- Se solicita permiso `POST_NOTIFICATIONS` al iniciar.
- Para detener la alarma, el usuario toca "Detener alarma" en la notificación.
- El almacenamiento local usa `Preferences` para persistir tareas.

