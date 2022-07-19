<script>
  export let time = 5
</script>
<style>
  .stats-container {
    display: flex;
    font-size: 0.6rem;
  }
  .average {
    text-transform: uppercase;
    
    margin-right: auto;
    margin-top: auto;
    margin-bottom: auto;
  }

  .time-diff {
    margin-left: auto;
    margin-top: auto;
    margin-bottom: auto;
  }

  .time-part{
    font-size: 1rem;
    color: #fff;
  }
</style>

<div class="stats-container">
  <span class="average">
    Average
  </span>
  <div class="time-diff">
    +/- <span class="time-part">{time}</span> MIN
  </div>
</div>