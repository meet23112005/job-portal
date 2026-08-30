import { RadioGroup, RadioGroupItem } from './ui/radio-group'
import { Label } from './ui/label'
import { Button } from './ui/button'
import { useDispatch, useSelector } from 'react-redux'
import { setFilters, setFilterKeyword, setFilterLocation, setCurrentPage } from '@/redux/jobSlice'

const fitlerData = [
    {
        fitlerType: "Location",
        array: ["Delhi NCR", "Bangalore", "Hydrabad", "Pune", "Mumbai","surat"]
    },
    {
        fitlerType: "Industry",
        array: ["Frontend Developer", "Backend Developer", "FullStack Developer"]
    },
]

const FilterCard = () => {
    const dispatch = useDispatch();
    const { filters } = useSelector(store => store.job);
    const selectedValue = filters.location || filters.keyword || '';

    const changeHandler = (value) => {
        const selectedFilterType = fitlerData.find((group) => group.array.includes(value))?.fitlerType;
        console.log('Filter selection:', selectedFilterType, value);

        if (selectedFilterType === 'Location') {
            dispatch(setFilterLocation(value));
        } else if (selectedFilterType === 'Industry') {
            dispatch(setFilterKeyword(value));
        }

        // Ensure we reset to page 1 so useGetAllJobs hook triggers the fetch
        dispatch(setCurrentPage(1));
    }

    const clearFilters = () => {
        dispatch(setFilters({ keyword: '', location: '' }));
    }

    return (
        <div className='w-full bg-white p-3 rounded-md'>
            <div className='flex items-center justify-between gap-3'>
                <h1 className='font-bold text-lg'>Filter Jobs</h1>
                <Button
                    type='button'
                    variant='outline'
                    size='sm'
                    onClick={clearFilters}
                    disabled={!selectedValue}
                >
                    Clear
                </Button>
            </div>
            <hr className='mt-3' />
            <RadioGroup value={selectedValue} onValueChange={changeHandler}>
                {
                    fitlerData.map((data, index) => (
                        // 1. Added unique key here using filterType
                        <div key={data.fitlerType}>
                            <h1 className='font-bold text-lg'>{data.fitlerType}</h1>
                            {
                                data.array.map((item, idx) => {
                                    const itemId = `id${index}-${idx}`
                                    return (
                                        // 2. Added unique key here using itemId
                                        <div
                                            key={itemId}
                                            className='flex items-center space-x-2 my-2 cursor-pointer'
                                            onClick={() => changeHandler(item)}
                                        >
                                            <RadioGroupItem value={item} id={itemId} />
                                            <Label htmlFor={itemId}>{item}</Label>
                                        </div>
                                    )
                                })
                            }
                        </div>
                    ))
                }
            </RadioGroup>
        </div>
    )
}

export default FilterCard