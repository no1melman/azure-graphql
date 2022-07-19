<script>
  export let highlight = false;

  let highlightClass = highlight ? 'highlight' : '';
</script>

<style>
  .highlight {
    background-color: #1c1f30;
  }
  .cell {
    padding: 2px;
  }
  .firstColumn {
    grid-column-start: 1;
    grid-column-end: 2;
  }
  .secondColumn {
    border-left: 1px solid #ebebef;
    grid-column-start: 2;
    grid-column-end: 3;
  }
  .thirdColumn {
    border-left: 1px solid #ebebef;
    grid-column-start: 3;
    grid-column-end: 4;
  }
  .fourthColumn {
    border-left: 1px solid #ebebef;
    grid-column-start: 4;
    grid-column-end: 5;
  }
  .fifthColumn {
    border-left: 1px solid #ebebef;
    grid-column-start: 5;
    grid-column-end: 6;
  }
  .sixthColumn {
    border-left: 1px solid #ebebef;
    grid-column-start: 6;
    grid-column-end: 7;
  }
  .seventhColumn {
    border-left: 1px solid #ebebef;
    grid-column-start: 7;
    grid-column-end: 8;
  }
</style>

<div class="firstColumn cell {highlightClass}"><slot name="first"></slot></div>
<div class="secondColumn cell {highlightClass}"><slot name="second"></slot></div>
<div class="thirdColumn cell {highlightClass}"><slot name="third"></slot></div>
<div class="fourthColumn cell {highlightClass}"><slot name="fourth"></slot></div>
<div class="fifthColumn cell {highlightClass}"><slot name="fifth"></slot></div>
<div class="sixthColumn cell {highlightClass}"><slot name="sixth"></slot></div>
<div class="seventhColumn cell {highlightClass}"><slot name="seventh"></slot></div>