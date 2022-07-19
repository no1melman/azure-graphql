<script>
  export let time = "00:00"


  let days = 0
  let hours = 0
  let mins = 0
  const hourMinuteRegex = /^(\d?\d)\:(\d\d)$/
  const dayHourMinuteRegex = /^(\d?\d)\:(\d\d)\:(\d\d)$/

  const isHourMinute = hourMinuteRegex.test(time)
  const isDayMinute = dayHourMinuteRegex.test(time)

  if (isHourMinute) {
    const parts = hourMinuteRegex.exec(time)

    hours = parseInt(parts[1], 10);
    mins = parseInt(parts[2], 10);
  }

  if (isDayMinute) {
    const parts = dayHourMinuteRegex.exec(time)

    days = parseInt(parts[1], 10)
    hours = parseInt(parts[2], 10);
    mins = parseInt(parts[3], 10);
  }

  const timeToShow = mins > 0 || hours > 0 || days > 0
  const shouldShowTime = (isDayMinute || isHourMinute) && timeToShow
  
</script>

<style>
  .number {
    font-size: 200%;
    font-weight: bold;
  }
  .timeofday {
    font-size: 100%;
  }
</style>

<div>
  {#if days > 0 && shouldShowTime }
  <span class="number">{days}</span>
  <span class="timeofday">d</span>
  {/if}
  {#if hours > 0 || days > 0 && shouldShowTime }
  <span class="number">{hours}</span>
  <span class="timeofday">h</span>
  {/if}
  {#if mins > 0 || hours > 0 && shouldShowTime}
  <span class="number">{mins}</span>
  <span class="timeofday">m</span>
  {:else}
  <span class="timeofday">no data...</span>
  {/if}
</div>