using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SmelterGame.Inventory.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField, Required]
        private InventoryCell _cellPrefab;
        [SerializeField, Required]
        private Transform _cellParent;
        [SerializeField, Required]
        private Canvas _cellDragCanvas;

        private ICellFactory _cellFactory;
        private Dictionary<Guid, InventoryCell> _filledCells = new();

        private void Awake()
        {
            _cellFactory = new DefaultCellFactory(_cellPrefab, _cellParent, _cellDragCanvas);
        }

        private void OnEnable()
        {
            InventoryManager.OnInventoryUpdated += InventoryUpdated;
        }

        private void OnDisable()
        {
            InventoryManager.OnInventoryUpdated -= InventoryUpdated;
        }

        private void InventoryUpdated(IItem item, int amount)
        {
            if (_filledCells.TryGetValue(item.GetID(), out var cell))
            {
                UpdateExistingCell(item, amount, cell);
            }
            else
            {
                CreateNewCellInstance(item, amount);
            }

            void UpdateExistingCell(IItem item, int amount, InventoryCell cell)
            {
                cell.UpdateCell(item, amount);
                if (cell.IsEmpty())
                {
                    _filledCells.Remove(item.GetID());
                }
            }

            void CreateNewCellInstance(IItem item, int amount)
            {
                InventoryCell cell = _cellFactory.Create(item, amount);
                _filledCells[item.GetID()] = cell;
            }
        }

        private interface ICellFactory
        {
            InventoryCell Create(IItem item, int amount);
        }

        private class DefaultCellFactory : ICellFactory
        {
            private readonly InventoryCell _cellPrefab;
            private readonly Transform _cellParent;
            private readonly Transform _cellDragParent;

            public DefaultCellFactory(InventoryCell cellPrefab, Transform cellParent, Canvas cellDragCanvas)
            {
                _cellPrefab = cellPrefab;
                _cellParent = cellParent;
                _cellDragParent = cellDragCanvas.transform;
            }

            public InventoryCell Create(IItem item, int amount)
            {
                var cellInstance = Instantiate(_cellPrefab, _cellParent);
                cellInstance.Initialize(_cellParent, _cellDragParent, item, amount);
                return cellInstance;
            }
        }
    }
}
