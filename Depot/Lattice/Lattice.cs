using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    /// <summary>
    /// Определяет минимальную сущность которую можно сохранить в базе данных.
    /// Эта сущность всегда описывает создание или модификацию объекта:
    /// 1. правила;
    /// 1.1. Факты
    /// 1.1.1. The NAME is a dictionary.
    /// 1.1.2. The NAME is a document's type.
    /// 1.1.3. The NAME is a accounting.
    /// 1.1.4. The NAME is a constant.
    /// 1.1.5. The NAME is a calendar.
    /// 1.2. Сообщения
    /// 1.3. Выводы
    /// 1.4. Действия
    /// 2. документа;
    /// 3. элемента справочника;
    /// 4. проводки в учете;
    /// 5. события в календаре;
    /// 6. значения констант
    /// В хранилище lattice представлена в виде набора более элементарных кусков - traits.
    /// Потомки данного класса определяют поведение бизнес сущностей.
    /// </summary>
    internal abstract class Lattice
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        protected LatticeId id;
        /// <summary>
        /// Уникальный внутренний идентификатор сущности к которой относится запись
        /// </summary>
        protected EntityId entityId;
        /// <summary>
        /// Guid сущности к которой относится запись
        /// </summary>
        protected Guid entityGuid;
        /// <summary>
        /// Дата и время физического создания записи
        /// </summary>
        protected DateTime createDate;
        /// <summary>
        /// Элементы из которых состоит запись
        /// </summary>
        List<Trait> traits;
    }
}
