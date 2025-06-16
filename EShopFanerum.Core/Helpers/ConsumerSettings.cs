namespace EShopFanerum.Core.Helpers
{
    /// <summary>
    /// Настройки потребителя сообщений.
    /// </summary>
    public class ConsumerSettings
    {
        /// <summary>
        /// Название, используется в массиве для настроек конечных групп консьюмеров. Для настроек по умолчанию не нужно.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Количество сообщений, отправляемых одновременно от брокера к потребителю до ожидания подтверждения.
        /// </summary>
        public ushort PrefetchCount { get; set; }

        /// <summary>
        /// Максимальное количество потоков.
        /// </summary>
        public int ConcurrentConsumerLimit { get; set; }
    }
}
